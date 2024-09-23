using System;
using System.Collections.Generic;
using Game;
using GBJAM12.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using Gemserk.Utilities.Signals;
using UnityEngine;

namespace GBJAM12.Scenes
{
    public class GameController : MonoBehaviour
    {
        public static int currentLevel;

        public GameConfiguration gameConfiguration;
        
        public WorldReference worldReference;
        public List<MusicLane> lanes;

        public AudioSource source;

        public MistakesUI mistakesUI;
        // on start, spawn current level

        public SignalAsset onAllMistakes;

        public void SpawnCurrentLevel()
        {
            var gameTrack = gameConfiguration.levels[currentLevel].GetInterface<GameTrackAssetV2>();
            
            foreach (var t in lanes)
            {
                t.midiDataAsset = gameTrack.midi;
            }
            
            foreach (var segment in gameTrack.segments)
            {
                for (var i = 0; i < lanes.Count; i++)
                {
                    var gameTrackLane = segment.laneAsset.lanes[i];
                    lanes[i].SpawnNotes(gameTrackLane.track, gameTrackLane.GetNotesArray(), segment.startCompass, segment.endCompass);
                }
            }
            
            source.clip = gameTrack.song;
        }

        private void Update()
        {
            var world = worldReference.GetReference(gameObject);

            if (world.TryGetSingletonEntity<DanceMovesComponent>(out var danceMovesEntity))
            {
                ref var danceMoves = ref danceMovesEntity.Get<DanceMovesComponent>();

                for (var i = 0; i < lanes.Count; i++)
                {
                    danceMoves.incomingNotes[i] = new IncomingNote()
                    {
                        hasIncomingNote = lanes[i].hasNotePlaying,
                        distanceToEndInTicks = lanes[i].distanceToEndNoteInTicks
                        // durationInTicks = 960
                    };
                }

                danceMoves.currentMistakes = 0;
                
                foreach (var lane in lanes)
                {
                    danceMoves.currentMistakes += lane.failedNotes;
                }

                mistakesUI.SetMistakes(danceMoves.totalMistakes, danceMoves.currentMistakes);

                if (danceMoves.previousMistakes < danceMoves.totalMistakes && danceMoves.currentMistakes >= danceMoves.totalMistakes)
                {
                    onAllMistakes.Signal(danceMovesEntity);
                }
                
                danceMoves.previousMistakes = danceMoves.currentMistakes;
            }
        }
    }
}