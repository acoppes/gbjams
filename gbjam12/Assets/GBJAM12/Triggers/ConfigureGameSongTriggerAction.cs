using Gemserk.Triggers;
using Gemserk.Utilities;
using UnityEngine;

namespace GBJAM12.Triggers
{
    public class ConfigureGameSongTriggerAction : TriggerAction
    {
        [ObjectType(typeof(GameTrackAsset), disableAssetReferences = true, filterString = "Configuration", prefabReferencesOnWhenStart = true)]
        public Object gameTrackConfiguration;

        public Transform lanesParent;
        public AudioSource source;
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var gameTrack = gameTrackConfiguration.GetInterface<GameTrackAsset>();

            var lanes = lanesParent.GetComponentsInChildren<MusicLane>();

            for (var i = 0; i < lanes.Length; i++)
            {
                var gameTrackLane = gameTrack.lanes[i];
                lanes[i].midiDataAsset = gameTrack.midi;
                lanes[i].SpawnNotes(gameTrackLane.track, gameTrackLane.GetNotesArray());
            }

            source.clip = gameTrack.song;

            return ITrigger.ExecutionResult.Completed;
        }
    }
}