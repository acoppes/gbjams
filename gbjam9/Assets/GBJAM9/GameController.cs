using System;
using System.Collections;
using System.Collections.Generic;
using GBJAM.Commons;
using GBJAM.Commons.Transitions;
using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9
{
    public class GameController : MonoBehaviour
    {
        public enum GameState
        {
            Idle,
            Fighting,
            TransitioningNextRoom,
            Restarting
        }
        
        public CameraFollow cameraFollow;

        public GameObject mainPlayerUnitPrefab;

        [NonSerialized]
        public UnitComponent mainPlayerUnitComponent;

        public GameObject mainMenuRoomPrefab;

        [NonSerialized]
        public Room currentRoom;
        
        public List<GameObject> roomPrefabs;

        public GameObject roomExitUnitPrefab;

        private List<UnitComponent> roomExitUnits = new List<UnitComponent>();

        public AudioSource backgroundMusicAudioSource;

        public AudioClip[] idleMusics;

        public AudioClip[] combatMusics;

        private GameState gameState = GameState.Idle;

        public GameObject transitionPrefab;

        public EntityManager entityManager;

        public GameboyButtonsUpdater inputUpdater;

        public float delayBetweenRooms = 0.5f;
        
        // TODO: more stuff

        public void Start()
        {
            // Start game sequence as coroutine?

            backgroundMusicAudioSource.loop = true;
            backgroundMusicAudioSource.clip = idleMusics[0];
            backgroundMusicAudioSource.Play();

            StartCoroutine(RestartGame(true));
        }

        private IEnumerator RestartGame(bool firstTime)
        {
            gameState = GameState.Restarting;

            yield return null;

            if (mainPlayerUnitComponent != null)
            {
                GameObject.Destroy(mainPlayerUnitComponent.gameObject);
                mainPlayerUnitComponent = null;
            }
            
            var unitObject = GameObject.Instantiate(mainPlayerUnitPrefab);
            mainPlayerUnitComponent = unitObject.GetComponent<UnitComponent>();
            cameraFollow.followTransform = mainPlayerUnitComponent.transform;
            
            if (currentRoom != null)
            {
                GameObject.Destroy(currentRoom);
            }

            var roomObject = GameObject.Instantiate(mainMenuRoomPrefab);
            currentRoom = roomObject.GetComponent<Room>();
            mainPlayerUnitComponent.transform.position = currentRoom.roomStart.transform.position;

            RegenerateRoomExits();

            gameState = GameState.Fighting;
        }

        private IEnumerator StartTransitionToNextRoom(RoomExitComponent roomExit)
        {
            gameState = GameState.TransitioningNextRoom;
            mainPlayerUnitComponent.GetComponentInChildren<UnitInput>().enabled = false;

            yield return null;

            var transitionObject = GameObject.Instantiate(transitionPrefab);
            // var transitionPosition = cameraFollow.cameraTransform.position;
            // transitionPosition.z = 0;
            transitionObject.transform.position = roomExit.transform.position;

            var transition = transitionObject.GetComponent<Transition>();
            transition.Open();

            yield return new WaitWhile(delegate
            {
                return !transition.isOpen;
            });

            yield return new WaitForSeconds(delayBetweenRooms);
            
            GameObject.Destroy(currentRoom.gameObject);

            var nextRoomPrefab = roomPrefabs[UnityEngine.Random.Range(0, roomPrefabs.Count)];
            var roomObject = GameObject.Instantiate(nextRoomPrefab);
            currentRoom = roomObject.GetComponent<Room>();
            mainPlayerUnitComponent.transform.position = currentRoom.roomStart.transform.position;
            
            transitionObject.transform.position = currentRoom.roomStart.transform.position;
            
            transition.Close();
            
            yield return new WaitWhile(delegate
            {
                return !transition.isClosed;
            });
            
            GameObject.Destroy(transition.gameObject);

            gameState = GameState.Fighting;
            
            RegenerateRoomExits();
            
            mainPlayerUnitComponent.GetComponentInChildren<UnitInput>().enabled = true;
        }

        private void RegenerateRoomExits()
        {
            foreach (var roomExitUnit in roomExitUnits)
            {
                GameObject.Destroy(roomExitUnit.gameObject);
            }
            
            roomExitUnits.Clear();
            
            foreach (var roomExit in currentRoom.roomExits)
            {
                var roomExitObject = GameObject.Instantiate(roomExitUnitPrefab);
                roomExitObject.transform.position = roomExit.transform.position;
                var roomExitUnit = roomExitObject.GetComponentInChildren<UnitComponent>();
                roomExitUnits.Add(roomExitUnit);
            }
        }
        
        public void Update()
        {
            if (gameState == GameState.TransitioningNextRoom)
            {
                return;
            }
        
            if (gameState == GameState.Restarting)
            {
                return;
            }

            // check if one room exit is pressed
            var roomExitList = entityManager.GetEntityList<RoomExitComponent>();
            foreach (var roomExit in roomExitList)
            {
                if (roomExit.mainUnitCollision)
                {
                    // TODO: more room data and logic..
                    StartCoroutine(StartTransitionToNextRoom(roomExit));
                }
            }
            
        }
        
    }
}