using System;
using System.Collections.Generic;
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

        public List<RoomRewardType> rewardTypes;
    }
}