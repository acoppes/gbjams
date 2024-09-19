using System;
using System.Linq;
using GBJAM12.Utilities;
using UnityEngine;

namespace GBJAM12
{
    public class DevMusicLaneOnStart : MonoBehaviour
    {
        public MusicLane musicLane;
        public MidiDataAsset midiDataAsset;
        public AudioSource musicTrack;
        public string trackName;

        public string notesArray;
        public int[] notes;

        private void Start()
        {
            notes = notesArray.Split(",").Select(int.Parse).ToArray();
            musicLane.SpawnNotes(midiDataAsset, musicTrack, trackName, notes);
        }
    }
}