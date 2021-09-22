using System;
using System.Collections.Generic;
using UnityEngine;

namespace GBJAM9
{
    public class GameController : MonoBehaviour
    {
        public enum GameState
        {
            Idle,
            Fighting,
            TransitioningNextRoom
        }
        
        public CameraFollow cameraFollow;

        public GameObject mainPlayerUnitPrefab;

        [NonSerialized]
        public Unit mainPlayerUnit;

        public GameObject mainMenuRoomPrefab;

        [NonSerialized]
        public Room currentRoom;
        
        public List<GameObject> roomPrefabs;

        public GameObject roomExitUnitPrefab;

        private List<Unit> roomExitUnits = new List<Unit>();

        public AudioSource backgroundMusicAudioSource;

        public AudioClip[] idleMusics;

        public AudioClip[] combatMusics;

        // TODO: more stuff

        public void Start()
        {
            // Start game sequence as coroutine?

            backgroundMusicAudioSource.loop = true;
            backgroundMusicAudioSource.clip = idleMusics[0];
            backgroundMusicAudioSource.Play();
        }

        public void Update()
        {
            // or use a state machine for the game??

            if (mainPlayerUnit == null)
            {
                var unitObject = GameObject.Instantiate(mainPlayerUnitPrefab);
                mainPlayerUnit = unitObject.GetComponent<Unit>();
                cameraFollow.followTransform = mainPlayerUnit.transform;
            }

            if (currentRoom == null)
            {
                var roomObject = GameObject.Instantiate(mainMenuRoomPrefab);
                currentRoom = roomObject.GetComponent<Room>();

                mainPlayerUnit.transform.position = currentRoom.roomStart.transform.position;
            }
            
            // if no enemies in the room
            // create room exits

            if (roomExitUnits.Count == 0)
            {
                foreach (var roomExit in currentRoom.roomExits)
                {
                    var roomExitObject = GameObject.Instantiate(roomExitUnitPrefab);
                    roomExitObject.transform.position = roomExit.transform.position;
                    var roomExitUnit = roomExitObject.GetComponentInChildren<Unit>();
                    roomExitUnits.Add(roomExitUnit);
                }
            }
        }
        
    }
}