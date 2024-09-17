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

            foreach (var track in midiFile.Tracks)
            {
                var midiTrack = new MidiDataAsset.MidiTrack();
                
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
                        midiTrack.textEvents.Add(new MidiDataAsset.MidiTrackTextEvent()
                        {
                            value = textEvent.Value,
                            time = textEvent.Time,
                            type = textEvent.TextEventType,
                        });
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