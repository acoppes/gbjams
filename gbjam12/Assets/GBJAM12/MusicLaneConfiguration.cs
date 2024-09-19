using UnityEngine;

namespace GBJAM12
{
    [CreateAssetMenu(menuName = "Music Lane Configuration", fileName = "MusicLaneConfiguration", order = 0)]
    public class MusicLaneConfiguration : ScriptableObject
    {
        public float distancePerTick = 1/16f;
        public int minDurationInTicksToShow = 960;
        public int latencyOffsetInTicks = 400;
    }
}