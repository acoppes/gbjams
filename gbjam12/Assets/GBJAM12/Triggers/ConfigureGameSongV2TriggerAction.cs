using Gemserk.Triggers;
using Gemserk.Utilities;
using UnityEngine;

namespace GBJAM12.Triggers
{
    public class ConfigureGameSongV2TriggerAction : TriggerAction
    {
        [ObjectType(typeof(GameTrackAssetV2), disableAssetReferences = true, filterString = "Configuration", prefabReferencesOnWhenStart = true)]
        public Object gameTrackConfiguration;

        public Transform lanesParent;
        public AudioSource source;
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var gameTrack = gameTrackConfiguration.GetInterface<GameTrackAssetV2>();

            var lanes = lanesParent.GetComponentsInChildren<MusicLane>();

            foreach (var t in lanes)
            {
                t.midiDataAsset = gameTrack.midi;
            }
            
            foreach (var segment in gameTrack.segments)
            {
                for (var i = 0; i < lanes.Length; i++)
                {
                    var gameTrackLane = segment.laneAsset.lanes[i];
                    lanes[i].SpawnNotes(gameTrackLane.track, gameTrackLane.GetNotesArray(), segment.startCompass, segment.endCompass);
                }
            }
            
            source.clip = gameTrack.song;

            return ITrigger.ExecutionResult.Completed;
        }
    }
}