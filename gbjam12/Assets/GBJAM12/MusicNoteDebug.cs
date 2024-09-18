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
        public int currentTick;
        
        private List<MidiNoteDebug> midiNoteDebugList = new List<MidiNoteDebug>();

        private void Awake()
        {
            GetComponentsInChildren(midiNoteDebugList);
        }

        private void Update()
        {
            var musicTimeInSeconds = music.time;
            
            currentTick = Mathf.FloorToInt(midiDataAsset.ticksPerSecond * musicTimeInSeconds);
            
            //
            // currentTick = Mathf.FloorToInt(musicTimeInSeconds / secondsPerTick);
            
            // seconds = ticks * seconds per tick
            
            foreach (var midiNoteDebug in midiNoteDebugList)
            {
                var track = midiDataAsset.GetByName(midiNoteDebug.track);
                
                foreach (var midiEvent in track.events)
                {
                    // var seconds = trackTimeInTicks * secondsPerTick;
                    //
                    // if (musicTimeInSeconds > seconds)
                    // {
                    //     break;
                    // }

                    if (midiEvent.note != midiNoteDebug.note)
                    {
                        continue;
                    }
                    
                    if (midiEvent.timeInTicks > currentTick)
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
