using UnityEngine;

namespace GBJAM12
{
    [CreateAssetMenu(menuName = "Music Lane Configuration", fileName = "MusicLaneConfiguration", order = 0)]
    public class MusicLaneConfiguration : ScriptableObject
    {
        private const int DefaultTicksPerQuarter = 960;
        
        public float distancePerTick = 1/16f;
        public int minDurationInTicksToShow = DefaultTicksPerQuarter / 2;
        public int latencyOffsetInTicks = DefaultTicksPerQuarter / 2;
        public int noteTicksThresholdToPress = DefaultTicksPerQuarter / 4;
    }
}