using System;
using System.Collections.Generic;
using MidiParser;
using UnityEngine;

namespace GBJAM12.Utilities
{
    public class MidiDataAsset : ScriptableObject
    {
        [Serializable]
        public class MidiTrackTextEvent
        {
            public int time;
            public TextEventType type;
            public string value;
        }
        
        [Serializable]
        public class MidiTrack
        {
            public string name;
            public List<MidiTrackTextEvent> textEvents = new List<MidiTrackTextEvent>();
        }

        public List<MidiTrack> tracks = new List<MidiTrack>();
    }
}