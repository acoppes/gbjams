using System;
using System.Collections.Generic;
using System.Linq;
using MidiParser;
using UnityEngine;
using UnityEngine.Serialization;

namespace GBJAM12.Utilities
{
    public class MidiDataAsset : ScriptableObject
    {
        [Serializable]
        public struct MidiTrackTextEvent
        {
            public int time;
            public TextEventType type;
            public string value;
        }
        
        [Serializable]
        public struct MidiEvent
        {
            [FormerlySerializedAs("time")] 
            public int timeInTicks;

            public int beatNumber;

            // public int absoluteTimeInTicks;

            public float timeInSeconds;
            
            public MidiEventType type;
            public int value;
            public int note;
            public int channel;
            public int velocity;
            public int bankSelect;
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

        public int ticksPerMinute => bpm * ppq;
        public int ticksPerSecond => ticksPerMinute / 60;
        
        //  var ticksPerMinute = midiDataAsset.bpm * midiDataAsset.ppq;
        // ticksPerSeconds = ticksPerMinute / 60f;
        
        public List<MidiTrack> tracks = new List<MidiTrack>();

        public MidiTrack GetByName(string trackName)
        {
            return tracks.FirstOrDefault(t => t.name.Equals(trackName, StringComparison.OrdinalIgnoreCase));
        }
    }
}