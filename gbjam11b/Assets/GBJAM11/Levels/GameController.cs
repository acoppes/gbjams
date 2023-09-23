using System.Collections.Generic;
using GBJAM11.LevelDesign;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace GBJAM11.Levels
{
    public class GameController : MonoBehaviour
    {
        public GameObject roomStartPrefab;
        public GameObject roomEndPrefab;
    
        public List<GameObject> roomPrefabs;

        public int total;

        private Room startingRoom;
        private Room exitRoom;
    
        private Room currentRoom;
        private Room nextRoom;
    
    
        // Start is called before the first frame update
        void Start()
        {
            var world = World.Default;

            var position = Vector2.zero;

            {
                var roomGameObject = GameObject.Instantiate(roomStartPrefab);
                roomGameObject.transform.position = position;
                var room = roomGameObject.GetComponent<Room>();
                position = room.exitPosition.position;

                currentRoom = room;
                startingRoom = room;
            
                ActivateRoom(roomGameObject);
            }
        
            var previousRoom = currentRoom;
        
            for (var i = 0; i < total; i++)
            {
                var roomGameObject = GameObject.Instantiate(roomPrefabs.Random());
                roomGameObject.transform.position = position;
                var room = roomGameObject.GetComponent<Room>();
                position = room.exitPosition.position;

                if (i == 0)
                {
                    nextRoom = room;
                }
            
                previousRoom.nextRoom = room;
                room.previousRoom = previousRoom;

                previousRoom = room;
            }   
        
            {
                var roomGameObject = GameObject.Instantiate(roomEndPrefab);
                roomGameObject.transform.position = position;

                exitRoom = roomGameObject.GetComponent<Room>();
            }
        }

        private void ActivateRoom(GameObject roomGameObject)
        {
            roomGameObject.transform.Find("LevelDesign").BroadcastMessage("InstantiateEntity", SendMessageOptions.DontRequireReceiver);
        }
    
        public void ActivateNextRoom()
        {
            nextRoom.transform.Find("LevelDesign").BroadcastMessage("InstantiateEntity", SendMessageOptions.DontRequireReceiver);
        
            // move to next room
            if (nextRoom.nextRoom != null)
            {
                currentRoom = nextRoom;
                nextRoom = nextRoom.nextRoom;
            }
        }
    }
}
