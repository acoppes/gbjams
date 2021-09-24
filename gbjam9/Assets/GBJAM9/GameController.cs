using System;
using System.Collections;
using System.Collections.Generic;
using GBJAM.Commons;
using GBJAM.Commons.Transitions;
using GBJAM9.Components;
using UnityEngine;
using UnityEngine.Serialization;

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
        public Entity mainPlayerEntity;

        public GameObject mainMenuRoomPrefab;

        [NonSerialized]
        public Room currentRoom;

        public RoomDataAsset rooms;
        
        public GameObject roomExitUnitPrefab;

        private List<Entity> roomExitUnits = new List<Entity>();

        public AudioSource backgroundMusicAudioSource;

        public AudioClip[] idleMusics;

        public AudioClip[] combatMusics;

        private GameState gameState = GameState.Idle;

        public GameObject transitionPrefab;

        [FormerlySerializedAs("entityManager")] public World world;

        public GameboyButtonsUpdater inputUpdater;

        public float delayBetweenRooms = 0.5f;
        
        // TODO: more stuff

        public void Start()
        {
            // Start game sequence as coroutine?

            if (backgroundMusicAudioSource != null)
            {
                backgroundMusicAudioSource.loop = true;
                backgroundMusicAudioSource.clip = idleMusics[0];
                backgroundMusicAudioSource.Play();
            }

            StartCoroutine(RestartGame(true));
        }

        private IEnumerator RestartGame(bool firstTime)
        {
            gameState = GameState.Restarting;

            yield return null;

            if (mainPlayerEntity != null)
            {
                GameObject.Destroy(mainPlayerEntity.gameObject);
                mainPlayerEntity = null;
            }
            
            var unitObject = GameObject.Instantiate(mainPlayerUnitPrefab);
            mainPlayerEntity = unitObject.GetComponent<Entity>();
            cameraFollow.followTransform = mainPlayerEntity.transform;
            
            if (currentRoom != null)
            {
                GameObject.Destroy(currentRoom);
            }

            var roomObject = GameObject.Instantiate(mainMenuRoomPrefab);
            currentRoom = roomObject.GetComponent<Room>();
            mainPlayerEntity.transform.position = currentRoom.roomStart.transform.position;
            
            roomObject.SendMessage("OnRoomStart", world);

            RegenerateRoomExits();

            gameState = GameState.Fighting;
        }

        private IEnumerator StartTransitionToNextRoom(RoomExitComponent roomExit)
        {
            gameState = GameState.TransitioningNextRoom;
            mainPlayerEntity.GetComponentInChildren<UnitInput>().enabled = false;

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

            var nextRoomPrefab = rooms.roomPrefabs[UnityEngine.Random.Range(0, rooms.roomPrefabs.Count)];
            var roomObject = GameObject.Instantiate(nextRoomPrefab);
            currentRoom = roomObject.GetComponent<Room>();
            mainPlayerEntity.transform.position = currentRoom.roomStart.transform.position;
            
            transitionObject.transform.position = currentRoom.roomStart.transform.position;
            
            transition.Close();
            
            yield return new WaitWhile(delegate
            {
                return !transition.isClosed;
            });
            
            GameObject.Destroy(transition.gameObject);

            gameState = GameState.Fighting;
            
            RegenerateRoomExits();
            
            mainPlayerEntity.GetComponentInChildren<UnitInput>().enabled = true;
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
                var roomExitUnit = roomExitObject.GetComponentInChildren<Entity>();
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
            var roomExitList = world.GetEntityList<RoomExitComponent>();
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