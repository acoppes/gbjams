﻿using System.Collections.Generic;
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

        public PlayerStatus playerStatus;
        public List<MusicLane> lanes;

        public AudioSource source;

        public MistakesUI mistakesUI;
        // on start, spawn current level

        public SignalAsset onAllMistakes;
        public SignalAsset onMistakeSignal;

        private GameTrackAssetV2 gameTrack;

        private int currentCompass = -1;

        public void SpawnCurrentLevel()
        {
            gameTrack = gameConfiguration.levels[currentLevel].GetInterface<GameTrackAssetV2>();
            
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
            // Debug compass changes
            if (source != null && source.isPlaying)
            {
                var time = source.time;
                var currentTick = Mathf.RoundToInt(gameTrack.midi.ticksPerSecond * time);
                var compass = gameTrack.midi.GetCurrentCompass(currentTick);
                
                if (currentCompass != compass)
                {
                    Debug.Log($"COMPASS CHANGE: {compass}, {time}s");
                    currentCompass = compass;
                }
            }
            
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

                danceMoves.currentMistakes = playerStatus.failedNotes;

                mistakesUI.SetMistakes(danceMoves.totalMistakes, danceMoves.currentMistakes);

                if (danceMoves.previousMistakes < danceMoves.currentMistakes)
                {
                    onMistakeSignal.Signal(danceMovesEntity);
                }

                if (danceMoves.previousMistakes < danceMoves.totalMistakes && danceMoves.currentMistakes >= danceMoves.totalMistakes)
                {
                    onAllMistakes.Signal(danceMovesEntity);
                }
                
                danceMoves.previousMistakes = danceMoves.currentMistakes;
            }
        }
    }
}