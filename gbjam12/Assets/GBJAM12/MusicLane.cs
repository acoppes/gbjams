using System;
using System.Collections.Generic;
using System.Linq;
using GBJAM12.Utilities;
using MidiParser;
using UnityEngine;

namespace GBJAM12
{
    public class MusicLane : MonoBehaviour
    {
        public MusicLaneConfiguration musicLaneConfiguration;
        
        public Transform notesParent;
        public GameObject notePrefab;
        
        public AudioSource songAudioSource;
        public MidiDataAsset midiDataAsset;

        [NonSerialized]
        public bool pressed;

        [NonSerialized]
        public int distanceToClosestIncomingNote;

        [NonSerialized]
        public int pressedTimeInTicks;
        
        [NonSerialized]
        public float pressedBuffer;

        private List<MusicLaneNote> laneNotes = new List<MusicLaneNote>();
        
        public void SpawnNotes(string trackName, int[] notes, float startCompass = 0, float endCompass = 99999)
        {
            var track = midiDataAsset.GetByName(trackName);
            var openNotes = new Dictionary<int, MusicLaneNote>();

            var compassDurationInTicks = midiDataAsset.ppq * 4;
            
            var startCompassInTicks = Mathf.RoundToInt(compassDurationInTicks * startCompass);
            var endCompassInTicks = Mathf.RoundToInt(compassDurationInTicks * endCompass);
            
            foreach (var midiEvent in track.events)
            {
                // ignore events outside valid compass
                if (midiEvent.timeInTicks < startCompassInTicks || midiEvent.timeInTicks > endCompassInTicks)
                {
                    continue;
                }
                
                if (midiEvent.type == MidiEventType.NoteOn)
                {
                    var note = midiEvent.note;
                    if (notes.Contains(note))
                    {
                        var noteInstance = GameObject.Instantiate(notePrefab, notesParent);
                        noteInstance.transform.localPosition =
                            new Vector3(0, (midiEvent.timeInTicks + musicLaneConfiguration.latencyOffsetInTicks) * musicLaneConfiguration.distancePerTick, 0);
                        noteInstance.SetActive(true);

                        var musicLaneNote = noteInstance.GetComponent<MusicLaneNote>();
                        
                        musicLaneNote.midiEvent = midiEvent;
                        musicLaneNote.durationInTicks = midiDataAsset.ppq / 2;
                        
                        openNotes[note] = musicLaneNote;
                        
                        laneNotes.Add(musicLaneNote);
                    }
                }
                
                if (midiEvent.type == MidiEventType.NoteOff)
                {
                    var note = midiEvent.note;
                    if (openNotes.ContainsKey(note))
                    {
                        var musicLaneNote = openNotes[note];
                        
                        musicLaneNote.durationInTicks = midiEvent.timeInTicks - musicLaneNote.midiEvent.timeInTicks;
                        
                        // var sixteenth = midiDataAsset.ppq / 4;
                        // musicLaneNote.durationInSixteenth = Mathf.RoundToInt(musicLaneNote.durationInTicks / (float) sixteenth);
                        
                        openNotes.Remove(note);
                    }
                }
            }
        }
        
        public void StorePressedInTicks()
        {
            pressedTimeInTicks = Mathf.RoundToInt(midiDataAsset.ticksPerSecond * songAudioSource.time);
        }
        
        public void Update()
        {
            // if (!wasPressed && pressed)
            // {
            //     wasPressed = true;
            //
            //     if (songAudioSource != null)
            //     {
            //         pressedTimeInTicks = Mathf.RoundToInt(midiDataAsset.ticksPerSecond * songAudioSource.time);
            //     }
            //
            // } else if (wasPressed && !pressed)
            // {
            //     wasPressed = false;
            // }
            
            // updates scroll based on track position
            if (songAudioSource != null && songAudioSource.isPlaying)
            {
                var time = songAudioSource.time;
                var currentTick = Mathf.RoundToInt(midiDataAsset.ticksPerSecond * time);

                notesParent.localPosition = new Vector3(0, -currentTick * musicLaneConfiguration.distancePerTick, 0);

                distanceToClosestIncomingNote = 9999;
                
                foreach (var note in laneNotes)
                {
                    var distanceToBePlayedInTicks = currentTick - musicLaneConfiguration.latencyOffsetInTicks -
                                                    note.midiEvent.timeInTicks;

                    if (distanceToBePlayedInTicks < 0 && Mathf.Abs(distanceToBePlayedInTicks) < distanceToClosestIncomingNote)
                    {
                        distanceToClosestIncomingNote = Mathf.Abs(distanceToBePlayedInTicks);
                    }
                    
                    // hasNoteInDistance = hasNoteInDistance || Mathf.Abs(distanceToBePlayedInTicks) < musicLaneConfiguration.noteTicksThresholdToPress;
                    
                    // var noteInDistanceToActivate = Mathf.Abs(currentTick - note.midiEvent.timeInTicks) < musicLaneConfiguration.noteTicksThresholdToPress;
                    
                    // var noteInDistanceToActivate = Mathf.Abs(currentTick - musicLaneConfiguration.latencyOffsetInTicks - note.midiEvent.timeInTicks) <
                    //                                musicLaneConfiguration.noteTicksThresholdToPress;

                    var distanceInTicks = pressedTimeInTicks - musicLaneConfiguration.latencyOffsetInTicks -
                                          note.midiEvent.timeInTicks;
                    
                    // I am before the note but inside some valid trheshold to activate?
                    var inDistanceToPress = distanceInTicks < 0 && Mathf.Abs(distanceInTicks) < musicLaneConfiguration.noteTicksThresholdToPress;

                    
                    
                    if (!note.isPressed && !note.wasActivated && pressed && inDistanceToPress)
                    {
                        note.isPressed = true;
                        note.wasActivated = true;
                    } else if (note.isPressed && !pressed)
                    {
                        note.isPressed = false;
                    }

                    if (note.isPressed)
                    {
                        note.activeTicks = 0;
                        var offset = currentTick - note.midiEvent.timeInTicks;
                        note.activeTicks = Mathf.Max(0, Mathf.Min(offset, note.durationInTicks));
                    }
                }
            }
        }


    }
}