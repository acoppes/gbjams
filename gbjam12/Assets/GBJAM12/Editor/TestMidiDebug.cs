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
                
                var json = JsonConvert.SerializeObject(midiFile, Formatting.Indented);
                Debug.Log(json);
                
            }
        }
        
    }
}