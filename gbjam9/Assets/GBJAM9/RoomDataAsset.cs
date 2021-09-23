using System.Collections.Generic;
using UnityEngine;

namespace GBJAM9
{
    [CreateAssetMenu(menuName = "GBJAM9/RoomList", fileName = "RoomDataAsset", order = 0)]
    public class RoomDataAsset : ScriptableObject
    {
        public string roomAssetsPath;
        public List<GameObject> roomPrefabs;
    }
}