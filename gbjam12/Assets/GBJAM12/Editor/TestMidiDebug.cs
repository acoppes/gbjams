using System.Collections.Generic;
using GBJAM12.Utilities;
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
        
        [MenuItem("GBJAM/Print Notes Per Track Per Compass")]
        public static void PrintNotesPerTrackPerCompass()
        {
            var selectedObject = Selection.activeObject;

            MidiDataAsset midiData = null;

            if (selectedObject is GameTrackAssetV2 gameTrack)
            {
                midiData = gameTrack.midi;
            }
            
            if (selectedObject is MidiDataAsset asset)
            {
                midiData = asset;
            }

            if (!midiData)
                return;
            
            foreach (var track in midiData.tracks)
            {
                var compass = 0;

                var notes = new HashSet<int>();
                    
                foreach (var midiEvent in track.events)
                {
                    var eventCompass = midiData.GetCurrentCompass(midiEvent.timeInTicks);

                    if (eventCompass != compass)
                    {
                        // var json = JsonConvert.SerializeObject(midiFile, Formatting.Indented);
                        Debug.Log($"{track.name}:{compass} [{string.Join(',', notes)}]");
                        notes = new HashSet<int>();
                        compass = eventCompass;
                    }
                            
                    if (midiEvent.type == MidiEventType.NoteOn || midiEvent.type == MidiEventType.NoteOff)
                    {
                        notes.Add(midiEvent.note);
                    }
                }
                    
                if (notes.Count > 0)
                {
                    Debug.Log($"{track.name}:{compass} => [{string.Join(',', notes)}]");
                }
            }
        }
        
    }
}