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

                // var absoluteTimeInTicks = 0;
                
                foreach (var midiEvent in track.MidiEvents)
                {
                    // absoluteTimeInTicks += midiEvent.Time;
                    
                    if (midiEvent.MidiEventType == MidiEventType.MetaEvent && midiDataAsset.bpm == 0)
                    {
                        if (midiEvent.MetaEventType == MetaEventType.Tempo)
                        {
                            midiDataAsset.bpm = midiEvent.Arg2;
                        }
                    }
                    
                    if (midiEvent.MidiEventType == MidiEventType.NoteOn)
                    {
                        midiTrack.events.Add(new MidiDataAsset.MidiEvent()
                        {
                            timeInTicks = midiEvent.Time,
                            beatNumber = midiEvent.Time / midiDataAsset.ppq,
                            // absoluteTimeInTicks = absoluteTimeInTicks,
                            timeInSeconds = midiDataAsset.ticksPerSecond > 0 ? midiEvent.Time / (float) midiDataAsset.ticksPerSecond : 0,
                            type = midiEvent.MidiEventType,
                            channel = midiEvent.Channel,
                            note = midiEvent.Note,
                            velocity = midiEvent.Velocity,
                        });
                    } else if (midiEvent.MidiEventType == MidiEventType.NoteOff)
                    {
                        midiTrack.events.Add(new MidiDataAsset.MidiEvent()
                        {
                            timeInTicks = midiEvent.Time,
                            beatNumber = midiEvent.Time / midiDataAsset.ppq,
                            // absoluteTimeInTicks = absoluteTimeInTicks,
                            timeInSeconds = midiDataAsset.ticksPerSecond > 0 ? midiEvent.Time / (float) midiDataAsset.ticksPerSecond : 0,
                            type = midiEvent.MidiEventType,
                            channel = midiEvent.Channel,
                            note = midiEvent.Note,
                            velocity = midiEvent.Velocity,
                        });
                    } else if (midiEvent.MidiEventType == MidiEventType.ControlChange)
                    {
                        midiTrack.events.Add(new MidiDataAsset.MidiEvent()
                        {
                            timeInTicks = midiEvent.Time,
                            beatNumber = midiEvent.Time / midiDataAsset.ppq,
                            // absoluteTimeInTicks = absoluteTimeInTicks,
                            timeInSeconds = midiDataAsset.ticksPerSecond > 0 ? midiEvent.Time / (float) midiDataAsset.ticksPerSecond : 0,
                            type = midiEvent.MidiEventType,
                            channel = midiEvent.Channel,
                            bankSelect = midiEvent.Arg2,
                            value= midiEvent.Value,
                        });
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