using System.Collections;
using System.Collections.Generic;
using GBJAM11.LevelDesign;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

public class TestRoomGeneration : MonoBehaviour
{
    public GameObject roomStartPrefab;
    public GameObject roomEndPrefab;
    
    public List<GameObject> roomPrefabs;

    public int total;
    
    // Start is called before the first frame update
    void Start()
    {
        var world = World.Default;

        var position = Vector2.zero;

        {
            var roomGameObject = GameObject.Instantiate(roomStartPrefab);
            roomGameObject.transform.position = position;
            var roomData = roomGameObject.GetComponent<RoomData>();
            position = roomData.exitPosition.position;
        }
        
        for (var i = 0; i < total; i++)
        {
            var roomGameObject = GameObject.Instantiate(roomPrefabs.Random());
            roomGameObject.transform.position = position;
            var roomData = roomGameObject.GetComponent<RoomData>();
            position = roomData.exitPosition.position;
        }   
        
        {
            var roomGameObject = GameObject.Instantiate(roomEndPrefab);
            roomGameObject.transform.position = position;
            var roomData = roomGameObject.GetComponent<RoomData>();
            position = roomData.exitPosition.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
