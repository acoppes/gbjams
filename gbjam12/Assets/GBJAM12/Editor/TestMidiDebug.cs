using System.Collections.Generic;
using MidiParser;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace GBJAM12.Editor
{
    public static class TestMidiDebug
    {
        [MenuItem("GBJAM/Print Selected Midi Info")]
        public static void PrintSelectedMidiInfo()
        {
            var selectedObject = Selection.activeObject;
            
            if (selectedObject)
            {
                var midiFile = new MidiFile(AssetDatabase.GetAssetPath(selectedObject));
                Debug.Log($"MIDI TRACKS: {midiFile.TracksCount}");
                Debug.Log($"MIDI FORMAT: {midiFile.Format}");
                Debug.Log($"MIDI TICKS: {midiFile.TicksPerQuarterNote}");

                foreach (var track in midiFile.Tracks)
                {
                    foreach (var midiEvent in track.MidiEvents)
                    {
                        if (midiEvent.MidiEventType == MidiEventType.MetaEvent)
                        {
                            Debug.Log($"Tempo: {midiEvent.Arg1}");
                            Debug.Log($"BPM: {midiEvent.Arg2}");
                        }
                    }
                }
                
                var json = JsonConvert.SerializeObject(midiFile, Formatting.Indented);
                Debug.Log(json);
                
            }
        }
        
        [MenuItem("GBJAM/Print Notes Per Track")]
        public static void PrintNotesPerTrack()
        {
            var selectedObject = Selection.activeObject;
            
            if (selectedObject)
            {
                var midiFile = new MidiFile(AssetDatabase.GetAssetPath(selectedObject));

                
                foreach (var track in midiFile.Tracks)
                {
                    var notes = new HashSet<int>();
                    var trackName = "";
                    
                    foreach (var textEvent in track.TextEvents)
                    {
                        if (textEvent.TextEventType == TextEventType.TrackName)
                        {
                            trackName = textEvent.Value;
                        }
                    }
                    
                    foreach (var midiEvent in track.MidiEvents)
                    {
                        if (midiEvent.MidiEventType == MidiEventType.NoteOn || midiEvent.MidiEventType == MidiEventType.NoteOff)
                        {
                            notes.Add(midiEvent.Note);
                        }
                    }
                    // var json = JsonConvert.SerializeObject(midiFile, Formatting.Indented);
                    Debug.Log($"{trackName}: [{string.Join(',', notes)}]");
                }
                
                
            }
        }
        
    }
}