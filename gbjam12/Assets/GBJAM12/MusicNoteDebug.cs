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

        public bool fillNotesOnStart;
        
        private void Awake()
        {
            GetComponentsInChildren(midiNoteDebugList);
        }
        
        private void Start()
        {
            if (fillNotesOnStart)
            {
                foreach (var midiNoteDebug in midiNoteDebugList)
                {
                    if (midiNoteDebug.notes.Length > 0)
                    {
                        continue;
                    }
                    
                    var track = midiDataAsset.GetByName(midiNoteDebug.track);

                    if (track == null)
                    {
                        continue;
                    }

                    var notes = new HashSet<int>();
                    foreach (var midiEvent in track.events)
                    {
                        if (midiEvent.type == MidiEventType.NoteOn || midiEvent.type == MidiEventType.NoteOff)
                        {
                            notes.Add(midiEvent.note);
                        }
                    }

                    midiNoteDebug.notes = notes.Select(n => new MidiNoteDebug.TrackNote()
                    {
                        note = n
                    }).ToArray();
                }
            }
        }

        private void Update()
        {
            var musicTimeInSeconds = music.time;
            
            currentTick = Mathf.CeilToInt(midiDataAsset.ticksPerSecond * musicTimeInSeconds);
            
            foreach (var midiNoteDebug in midiNoteDebugList)
            {
                var track = midiDataAsset.GetByName(midiNoteDebug.track);
                
                foreach (var midiEvent in track.events)
                {
                    if (midiEvent.timeInTicks > currentTick)
                    {
                        break;
                    }
                    
                    foreach (var trackNote in midiNoteDebug.notes)
                    {
                        if (midiEvent.note == trackNote.note)
                        {
                            if (midiEvent.type == MidiEventType.NoteOn)
                                trackNote.on = true;
                            else if (midiEvent.type == MidiEventType.NoteOff)
                                trackNote.on = false;
                        }
                    }
                }
            }
        }
    }
}
