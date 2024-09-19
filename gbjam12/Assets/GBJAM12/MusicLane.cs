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

        private AudioSource musicTrack;
        private MidiDataAsset midiDataAsset;

        public bool isActive;
        
        public void SpawnNotes(MidiDataAsset midiDataAsset, AudioSource musicTrack, string trackName, int[] notes)
        {
            this.midiDataAsset = midiDataAsset;
            this.musicTrack = musicTrack;
            
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
                    }
                }
                
                if (midiEvent.type == MidiEventType.NoteOff)
                {
                    var note = midiEvent.note;
                    if (openNotes.ContainsKey(note))
                    {
                        var musicLaneNote = openNotes[note];
                        
                        musicLaneNote.durationInTicks = midiEvent.timeInTicks - musicLaneNote.midiEvent.timeInTicks;
                        var sixteenth = midiDataAsset.ppq / 4;
                        musicLaneNote.durationInSixteenth = Mathf.RoundToInt(musicLaneNote.durationInTicks / (float) sixteenth);
                        
                        openNotes.Remove(note);
                    }
                }
            }
        }

        public void LateUpdate()
        {
            // updates scroll based on track position
            if (musicTrack != null)
            {
                var time = musicTrack.time;
                var currentTick = Mathf.RoundToInt(midiDataAsset.ticksPerSecond * time);

                notesParent.localPosition = new Vector3(0, -currentTick * musicLaneConfiguration.distancePerTick, 0);
            }

            inactiveImage.enabled = !isActive;
            activeImage.enabled = isActive;
            
        }
    }
}