using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GBJAM.Commons;
using GBJAM.Commons.Transitions;
using GBJAM9.Components;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GBJAM9
{
    public class GameController : MonoBehaviour
    {
        public CameraFollow cameraFollow;

        public GameObject mainPlayerUnitPrefab;

        public GameObject mainMenuRoomPrefab;

        public GameObject nekoSamaRoomPrefab;
        
        public RoomDataAsset rooms;
        
        public GameObject roomExitUnitPrefab;
        
        public AudioSource backgroundMusicAudioSource;
        
        public GameObject transitionPrefab;

        [FormerlySerializedAs("entityManager")] 
        public World world;

        public GameboyButtonsUpdater inputUpdater;

        public float delayBetweenRooms = 0.5f;

        public int minRooms, maxRooms;

        private int totalRooms;
        private Entity nekoninEntity;
        private RoomComponent currentRoom;
        private List<Entity> roomExitUnits = new List<Entity>();
        private Entity gameEntity;

        private Entity hud;
        
        public int initialHealth = 2;
        
        public void Start()
        {
            gameEntity = world.GetSingleton("Game");
            hud = world.GetSingleton("GameHud");
            
            // This controller could be an entity too...

            // Start game sequence as coroutine?
            StartCoroutine(RestartGame(true));
        }
        
        
        private void RestartMusic()
        {
            var musicClip = currentRoom.completedMusic;
            
            if (gameEntity.game.state == GameComponent.State.Fighting)
            {
                musicClip = currentRoom.fightMusic;
            }
            
            if (backgroundMusicAudioSource != null)
            {
                backgroundMusicAudioSource.loop = true;

                if (backgroundMusicAudioSource.clip == musicClip 
                    && backgroundMusicAudioSource.isPlaying)
                {
                    return;
                }

                backgroundMusicAudioSource.clip = musicClip;
                backgroundMusicAudioSource.Play();
            }
        }

        private IEnumerator RestartGame(bool firstTime)
        {
            gameEntity.game.state = GameComponent.State.Restarting;

            GameObject transitionObject = null;

            hud.hud.visible = false;
            
            if (!firstTime)
            {
                transitionObject = GameObject.Instantiate(transitionPrefab);
                transitionObject.transform.position = nekoninEntity.transform.position;

                var transition = transitionObject.GetComponent<Transition>();
                transition.Open();

                yield return new WaitWhile(delegate
                {
                    return !transition.isOpen;
                });

                yield return new WaitForSeconds(delayBetweenRooms);
            }

            if (nekoninEntity != null)
            {
                GameObject.Destroy(nekoninEntity.gameObject);
                nekoninEntity = null;
            }
            
            var unitObject = GameObject.Instantiate(mainPlayerUnitPrefab);
            nekoninEntity = unitObject.GetComponent<Entity>();
            cameraFollow.followTransform = nekoninEntity.transform;

            nekoninEntity.health.total = initialHealth;
            
            if (currentRoom != null)
            {
                GameObject.Destroy(currentRoom.gameObject);
            }

            var roomObject = GameObject.Instantiate(mainMenuRoomPrefab);
            currentRoom = roomObject.GetComponent<RoomComponent>();
            nekoninEntity.transform.position = currentRoom.roomStart.transform.position;

            if (!firstTime)
            {
                transitionObject.transform.position = currentRoom.roomStart.transform.position;
                var transition = transitionObject.GetComponent<Transition>();
                
                transition.Close();
            
                yield return new WaitWhile(delegate
                {
                    return !transition.isClosed;
                });
            
                GameObject.Destroy(transition.gameObject);
            }
            
            hud.hud.visible = true;
            
            totalRooms = UnityEngine.Random.Range(minRooms, maxRooms);

            gameEntity.game.state = GameComponent.State.Fighting;

            RegenerateRoomExits();
            RestartMusic();
        }


        private IEnumerator StartTransitionToNextRoom(RoomExitComponent roomExit)
        {
            hud.hud.visible = false;
            
            gameEntity.game.state = GameComponent.State.TransitionToRoom;
            
            nekoninEntity.GetComponentInChildren<UnitInput>().enabled = false;

            yield return null;

            var transitionObject = GameObject.Instantiate(transitionPrefab);
            // var transitionPosition = cameraFollow.cameraTransform.position;
            // transitionPosition.z = 0;
            transitionObject.transform.position = roomExit.transform.position;

            var nextRoomRewardType = roomExit.rewardType;

            var transition = transitionObject.GetComponent<Transition>();
            transition.Open();

            yield return new WaitWhile(delegate
            {
                return !transition.isOpen;
            });

            yield return new WaitForSeconds(delayBetweenRooms);
            
            GameObject.Destroy(currentRoom.gameObject);

            var nextRoomPrefab = rooms.roomPrefabs[UnityEngine.Random.Range(0, rooms.roomPrefabs.Count)];

            if (totalRooms == 0)
            {
                nextRoomPrefab = nekoSamaRoomPrefab;
            }

            var roomObject = GameObject.Instantiate(nextRoomPrefab);
            currentRoom = roomObject.GetComponent<RoomComponent>();
            nekoninEntity.transform.position = currentRoom.roomStart.transform.position;
            
            totalRooms--;

            currentRoom.rewardType = nextRoomRewardType;
            
            gameEntity.game.state = GameComponent.State.Fighting;

            RestartMusic();

            transitionObject.transform.position = currentRoom.roomStart.transform.position;
            transition.Close();
            
            yield return new WaitWhile(delegate
            {
                return !transition.isClosed;
            });
            
            GameObject.Destroy(transition.gameObject);

            RegenerateRoomExits();
            
            nekoninEntity.GetComponentInChildren<UnitInput>().enabled = true;
            
            hud.hud.visible = true;
        }

        private void RegenerateRoomExits()
        {
            // foreach (var roomExitUnit in roomExitUnits)
            // {
            //     // now is autodestroyed 
            //     if (roomExitUnit != null)
            //     {
            //         GameObject.Destroy(roomExitUnit.gameObject);
            //     }
            // }
            
            roomExitUnits.Clear();

            var roomExits = new List<RoomExitSpawn>(currentRoom.roomExits);

            var newRoomRewardTypes = rooms.rewardTypes.OrderBy(s => Random.value).ToList();

            for (var i = 0; i < roomExits.Count; i++)
            {
                var roomExit = roomExits[i];
                var roomExitObject = GameObject.Instantiate(roomExitUnitPrefab, roomExit.transform.position, 
                    Quaternion.identity, currentRoom.transform);
                // roomExitObject.transform.position = roomExit.transform.position;
                var roomExitUnit = roomExitObject.GetComponentInChildren<Entity>();
                roomExitUnits.Add(roomExitUnit);

                // if no more rooms, avoid generating next room reward
                if (newRoomRewardTypes.Count > 0)
                {
                    if (i >= newRoomRewardTypes.Count)
                    {
                        i = 0;
                    }
                    var rewardType = newRoomRewardTypes[i];
                    roomExitUnit.roomExit.rewardType = rewardType.name;
                }

                if (totalRooms <= 0)
                {
                    roomExitUnit.roomExit.rewardType = "unknown";
                }

                GameObject.Destroy(roomExit.gameObject);
            }
        }
        
        private IEnumerator VictorySequence()
        {
            gameEntity.game.state = GameComponent.State.TransitionToRoom;
            nekoninEntity.input.enabled = false;

            // TODO: show custom defeat screen, wait a bit, then go to restart game.
            
            yield return new WaitForSeconds(2.0f);

            StartCoroutine(RestartGame(false));
        }

          private IEnumerator DefeatSequence()
        {
            gameEntity.game.state = GameComponent.State.TransitionToRoom;
            nekoninEntity.input.enabled = false;
            
            // TODO: show custom defeat screen, wait a bit, then go to restart game.
            
            yield return new WaitForSeconds(2.0f);

            StartCoroutine(RestartGame(false));
            
            // yield return ;
        }
          
        public void Update()
        {
            if (gameEntity.game.state == GameComponent.State.TransitionToRoom)
            {
                return;
            }

            if (gameEntity.game.state == GameComponent.State.Restarting)
            {
                return;
            }

            if (gameEntity.game.state == GameComponent.State.Victory)
            {
                // TODO: start another sequence first (transition, stuff), then restart
                StartCoroutine(VictorySequence());
                return;
            }
            
            if (gameEntity.game.state == GameComponent.State.Defeat)
            {
                // TODO: start another sequence first (transition, stuff), then restart
                // StartCoroutine(RestartGame(false));
                StartCoroutine(DefeatSequence());
                return;
            }

            if (gameEntity.game.state == GameComponent.State.Fighting)
            {
                if (!nekoninEntity.health.alive)
                {
                    gameEntity.game.state = GameComponent.State.Defeat;
                    // StartCoroutine(DefeatSequence());
                    return;
                }
            }

                // check if one room exit is pressed
            var roomExitList = world.GetComponentList<RoomExitComponent>();
            foreach (var roomExit in roomExitList)
            {
                if (roomExit.playerInExit)
                {
                    // TODO: more room data and logic..
                    StartCoroutine(StartTransitionToNextRoom(roomExit));
                }
            }
            
        }
        
    }
}