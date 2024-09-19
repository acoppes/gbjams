using System;
using System.Linq;
using GBJAM12.Utilities;
using MidiParser;
using UnityEngine;

namespace GBJAM12
{
    public class MusicLane : MonoBehaviour
    {
        public Transform notesParent;
        public GameObject notePrefab;
        public float distancePerTick = 0.01f;

        public int latencyOffsetInTicks = 0;

        private AudioSource musicTrack;
        private MidiDataAsset midiDataAsset;
        
        public void Spawn(MidiDataAsset midiDataAsset, AudioSource musicTrack, string trackName, int[] notes)
        {
            this.midiDataAsset = midiDataAsset;
            this.musicTrack = musicTrack;
            
            var track = midiDataAsset.GetByName(trackName);
            foreach (var midiEvent in track.events)
            {
                if (midiEvent.type == MidiEventType.NoteOn)
                {
                    var note = midiEvent.note;
                    if (notes.Contains(note))
                    {
                        var noteInstance = GameObject.Instantiate(notePrefab, notesParent);
                        noteInstance.transform.localPosition =
                            new Vector3(0, (midiEvent.timeInTicks + latencyOffsetInTicks) * distancePerTick, 0);
                        noteInstance.SetActive(true);
                    }
                }
            }
        }

        public void LateUpdate()
        {
            // internalTimeUsingDt += Time.deltaTime;
            
            var time = musicTrack.time;
            var currentTick = Mathf.RoundToInt(midiDataAsset.ticksPerSecond * time);
            
            notesParent.localPosition = new Vector3(0, -currentTick * distancePerTick, 0);
        }
    }
}