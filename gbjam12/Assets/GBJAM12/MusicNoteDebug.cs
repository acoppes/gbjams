using System;
using System.Collections.Generic;
using System.Linq;
using GBJAM12.Utilities;
using MidiParser;
using MyBox;
using UnityEngine;

namespace GBJAM12
{
    public class MusicNoteDebug : MonoBehaviour
    {
        public AudioSource music;

        public MidiDataAsset midiDataAsset;

        [ReadOnly]
        public int currentBeat;

        [ReadOnly]
        public int currentTick;

        public float millisecondsPerTick;
        public float secondsPerTick;
        
        private List<MidiNoteDebug> midiNoteDebugList = new List<MidiNoteDebug>();

        private void Awake()
        {
            GetComponentsInChildren(midiNoteDebugList);
        }

        private void Update()
        {
            var musicTimeInSeconds = music.time;

            var beatsPerSecond = midiDataAsset.bpm * 60;
            currentBeat = Mathf.FloorToInt(beatsPerSecond * musicTimeInSeconds);

            // var t = 60000 / (midiDataAsset.bpm * midiDataAsset.ppq);
            // ticks per quarter = ppq
            // ms per quarter = tempo

            millisecondsPerTick = (float) midiDataAsset.bpm / (float) midiDataAsset.ppq;
            secondsPerTick = millisecondsPerTick / 100000f;
            
            currentTick = Mathf.FloorToInt(musicTimeInSeconds / secondsPerTick);
            
            // seconds = ticks * seconds per tick
            
            foreach (var midiNoteDebug in midiNoteDebugList)
            {
                var track = midiDataAsset.GetByName(midiNoteDebug.track);

                var trackTimeInTicks = 0;
                
                foreach (var midiEvent in track.events)
                {
                    if (midiEvent.note != midiNoteDebug.note)
                    {
                        continue;
                    }
                    
                    trackTimeInTicks += midiEvent.time;

                    if (trackTimeInTicks > currentBeat)
                    {
                        break;
                    }

                    if (midiEvent.type == MidiEventType.NoteOn)
                        midiNoteDebug.isPlaying = true;
                    else if (midiEvent.type == MidiEventType.NoteOff)
                        midiNoteDebug.isPlaying = false;
                }
                
                if (midiNoteDebug.isPlaying)
                {
                    break;
                }
            }
        }
    }
}
