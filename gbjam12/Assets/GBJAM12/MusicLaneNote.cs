using System;
using GBJAM12.Utilities;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

namespace GBJAM12
{
    public class MusicLaneNote : MonoBehaviour
    {
        public MusicLaneConfiguration musicLaneConfiguration;
        
        [NonSerialized]
        public int durationInTicks;
        
        // [NonSerialized]
        // public int durationInSixteenth;
        
        // [ReadOnly]
        // public int durationInSeconds;
        
        [NonSerialized]
        public MidiDataAsset.MidiEvent midiEvent;

        [NonSerialized]
        public bool isPressed;

        [NonSerialized]
        public bool wasActivated;

        [NonSerialized]
        public int activeTicks = 0;
    }
}