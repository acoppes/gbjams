using System;
using GBJAM12.Utilities;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

namespace GBJAM12
{
    public class MusicLaneNote : MonoBehaviour
    {
        public MusicLaneConfiguration musicLaneConfiguration;
        
        [ReadOnly]
        public int durationInTicks;
        
        [ReadOnly]
        public int durationInSixteenth;
        
        // [ReadOnly]
        // public int durationInSeconds;
        
        [NonSerialized]
        public MidiDataAsset.MidiEvent midiEvent;

        public Image inactiveDuration;
        public Image activeDuration;
        public Image activeNote;
        
        private void Start()
        {
            inactiveDuration.gameObject.SetActive(false);
            activeDuration.gameObject.SetActive(false);
            activeNote.gameObject.SetActive(false);
            
            if (durationInSixteenth >= musicLaneConfiguration.minDurationInFragmentToShow)
            {
                inactiveDuration.gameObject.SetActive(true);
                // activeDuration.gameObject.SetActive(true);
                
                inactiveDuration.rectTransform.SetHeight(musicLaneConfiguration.distancePerTick * durationInTicks);
                activeDuration.rectTransform.SetHeight(musicLaneConfiguration.distancePerTick * durationInTicks);
            }
        }
    }
}