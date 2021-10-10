using System;
using System.Collections.Generic;
using System.Linq;
using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9
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
            return roomPrefabs[UnityEngine.Random.Range(0, roomPrefabs.Count)];
        }
    }
}