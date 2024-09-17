using GBJAM12.Utilities;
using MidiParser;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace GBJAM12.Editor
{
    [ScriptedImporter(1, "mid")]
    public class MidiFileImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var midiDataAsset = ScriptableObject.CreateInstance<MidiDataAsset>();
            var midiFile = new MidiFile(ctx.assetPath);

            midiDataAsset.ppq = midiFile.TicksPerQuarterNote;

            // 60000 / (BPM * PPQ) 
            // midiDataAsset.bpm = 60000 / midiDataAsset.ppq;

            foreach (var track in midiFile.Tracks)
            {
                var midiTrack = new MidiDataAsset.MidiTrack();
                
                foreach (var midiEvent in track.MidiEvents)
                {
                    var trackMidiEvent = new MidiDataAsset.MidiEvent()
                    {
                        value = midiEvent.Value,
                        time = midiEvent.Time,
                        type = midiEvent.MidiEventType,
                        note = midiEvent.Note
                    };

                    if (trackMidiEvent.type == MidiEventType.NoteOn)
                    {
                        midiTrack.events.Add(trackMidiEvent);
                    } else if (trackMidiEvent.type == MidiEventType.NoteOff)
                    {
                        midiTrack.events.Add(trackMidiEvent);
                    }
                    
                    if (trackMidiEvent.type == MidiEventType.MetaEvent && midiDataAsset.tempo == 0)
                    {
                        midiDataAsset.tempo = midiEvent.Arg1;
                        midiDataAsset.bpm = midiEvent.Arg2;
                    }
                }
                
                foreach (var textEvent in track.TextEvents)
                {
                    var trackEvent = new MidiDataAsset.MidiTrackTextEvent()
                    {
                        value = textEvent.Value,
                        time = textEvent.Time,
                        type = textEvent.TextEventType,
                    };

                    if (trackEvent.type == TextEventType.TrackName)
                    {
                        midiTrack.name = trackEvent.value;
                    }
                    else
                    {
                        midiTrack.textEvents.Add(trackEvent);
                    }
                }
                
                midiDataAsset.tracks.Add(midiTrack);
            }
            
            // 'cube' is a GameObject and will be automatically converted into a prefab
            // (Only the 'Main Asset' is eligible to become a Prefab.)
            ctx.AddObjectToAsset("main obj", midiDataAsset);
            ctx.SetMainObject(midiDataAsset);
        }
    }
}