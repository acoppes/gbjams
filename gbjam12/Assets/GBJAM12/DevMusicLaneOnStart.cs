using System;
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
        public int[] notes;

        private void Start()
        {
            musicLane.Spawn(midiDataAsset, musicTrack, trackName, notes);
        }
    }
}