using System;
using UnityEngine;

namespace GBJAM9
{
    public class GameController : MonoBehaviour
    {
        public CameraFollow cameraFollow;

        public GameObject mainPlayerUnitPrefab;

        [NonSerialized]
        public Unit mainPlayerUnit;

        public GameObject mainMenuRoomPrefab;

        [NonSerialized]
        public Room currentRoom;
        
        // TODO: more stuff

        public void Start()
        {
            // Start game sequence...
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
        }
        
    }
}