using System;
using System.Collections.Generic;
using UnityEngine;

namespace GBJAM10
{
    [CreateAssetMenu(menuName = "GBJAM9/RoomList", fileName = "RoomDataAsset", order = 0)]
    public class RoomDataAsset : ScriptableObject
    {
        [Serializable]
        public class RoomRewardType
        {
            public string name;
            public GameObject prefab;
        }

        public string roomAssetsPath;
        public List<GameObject> roomPrefabs;

        public GameObject startingRoomPrefab;
        public GameObject endingRoomPrefab;

        public List<RoomRewardType> rewardTypes;

        public GameObject GetNextRoom(CurrentRunData runData)
        {
            // if runData.secret
            if (runData.totalRooms <= 0)
            {
                return endingRoomPrefab;
            }
            
            // if (runData.secretRooms == 
            // roomPrefabs.Where(g => g.GetComponent<RoomComponent>())
            
            return roomPrefabs[UnityEngine.Random.Range(0, roomPrefabs.Count)];
        }
    }
}