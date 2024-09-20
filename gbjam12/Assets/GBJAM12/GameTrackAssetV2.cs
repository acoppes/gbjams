using System;
using System.Collections.Generic;
using GBJAM12.Utilities;
using UnityEngine;

namespace GBJAM12
{
    public class GameTrackAssetV2 : MonoBehaviour
    {
        [Serializable]
        public class LaneSegment
        {
            public int startCompass;
            public GameTrackLaneAsset laneAsset;
        }

        public AudioClip song;
        public MidiDataAsset midi;

        public List<LaneSegment> segments;
    }
}