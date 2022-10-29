using System.Collections.Generic;
using UnityEngine;

namespace Beatemup
{
    [CreateAssetMenu(menuName = "GBJAM10/Chunks", fileName = "LevelChunks", order = 0)]
    public class LevelDataAsset : ScriptableObject
    {
        public string chunksPrefabsPath;
        public List<GameObject> chunksList;

        public GameObject startingChunk;
    }
}