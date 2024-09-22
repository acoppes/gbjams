using System.Collections.Generic;
using Gemserk.Utilities;
using UnityEngine;

namespace GBJAM12
{
    [CreateAssetMenu(menuName = "Music Lane Configuration", fileName = "MusicLaneConfiguration", order = 0)]
    public class GameConfiguration : ScriptableObject
    {
        private const int DefaultTicksPerQuarter = 960;
        
        public float distancePerTick = 1/16f;
        public int minDurationInTicksToShow = DefaultTicksPerQuarter / 2;
        public int latencyOffsetInTicks = DefaultTicksPerQuarter / 2;
        public int noteTicksThresholdToPress = DefaultTicksPerQuarter / 4;
        
        public float pressedTimeBuffer;

        [ObjectType(typeof(GameTrackAssetV2), filterString = "Configuration", prefabReferencesOnWhenStart = true, sceneReferencesOnWhenStart = false)]
        public List<Object> levels;
    }
}