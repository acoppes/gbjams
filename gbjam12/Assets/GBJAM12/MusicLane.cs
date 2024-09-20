using System;
using System.Collections.Generic;
using System.Linq;
using GBJAM12.Utilities;
using MidiParser;
using UnityEngine;
using UnityEngine.UI;

namespace GBJAM12
{
    public class MusicLane : MonoBehaviour
    {
        public MusicLaneConfiguration musicLaneConfiguration;
        
        public Transform notesParent;
        public GameObject notePrefab;

        public Image inactiveImage;
        public Image activeImage;

        public AudioSource songAudioSource;
        private MidiDataAsset midiDataAsset;

        // [NonSerialized]
        public bool buttonPressed => pressedBuffer > 0;

        [NonSerialized]
        public float pressedBuffer;

        private List<MusicLaneNote> laneNotes = new List<MusicLaneNote>();
        
        public void SpawnNotes(MidiDataAsset midiDataAsset, string trackName, int[] notes)
        {
            this.midiDataAsset = midiDataAsset;
            
            var track = midiDataAsset.GetByName(trackName);
            var openNotes = new Dictionary<int, MusicLaneNote>();
            
            foreach (var midiEvent in track.events)
            {
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
        
        public void Update()
        {
            // updates scroll based on track position
            if (songAudioSource != null)
            {
                var time = songAudioSource.time;
                var currentTick = Mathf.RoundToInt(midiDataAsset.ticksPerSecond * time);

                notesParent.localPosition = new Vector3(0, -currentTick * musicLaneConfiguration.distancePerTick, 0);
                
                foreach (var note in laneNotes)
                {
                    // var noteInDistanceToActivate = Mathf.Abs(currentTick - note.midiEvent.timeInTicks) < musicLaneConfiguration.noteTicksThresholdToPress;
                    
                    var noteInDistanceToActivate = Mathf.Abs(currentTick - musicLaneConfiguration.latencyOffsetInTicks - note.midiEvent.timeInTicks) <
                                                   musicLaneConfiguration.noteTicksThresholdToPress;
                    
                    if (!note.isPressed && !note.wasActivated && buttonPressed && noteInDistanceToActivate)
                    {
                        note.isPressed = true;
                        note.wasActivated = true;
                    } else if (note.isPressed && !buttonPressed)
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

            inactiveImage.enabled = !buttonPressed;
            activeImage.enabled = buttonPressed;
        }
    }
}