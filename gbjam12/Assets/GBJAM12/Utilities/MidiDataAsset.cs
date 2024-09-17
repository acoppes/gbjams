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
        public class MidiEvent
        {
            public int time;
            public MidiEventType type;
            public int value;
            public int note;
        }
        
        [Serializable]
        public class MidiTrack
        {
            public string name;
            
            
            public List<MidiTrackTextEvent> textEvents = new List<MidiTrackTextEvent>();
            public List<MidiEvent> events = new List<MidiEvent>();
        }
        
        public int ppq;
        public int bpm;
        public int tempo;
        
        public List<MidiTrack> tracks = new List<MidiTrack>();
    }
}