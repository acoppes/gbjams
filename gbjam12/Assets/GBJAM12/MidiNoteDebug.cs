using System;
using UnityEngine;
using UnityEngine.UI;

namespace GBJAM12
{
    public class MidiNoteDebug : MonoBehaviour
    {
        [Serializable]
        public class TrackNote
        {
            public int note;
            
            [NonSerialized]
            public bool on = false;
        }
        
        public string track;

        public TrackNote[] notes;

        public Image image;

        private bool isPlaying;

        private void LateUpdate()
        {
            isPlaying = false;
            
            for (var i = 0; i < notes.Length; i++)
            {
                isPlaying = isPlaying || notes[i].on;
            }
            
            image.enabled = isPlaying;
        }
    }
}