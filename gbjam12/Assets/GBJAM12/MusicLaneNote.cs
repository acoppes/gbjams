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
            
            if (durationInTicks >= musicLaneConfiguration.minDurationInTicksToShow)
            {
                inactiveDuration.gameObject.SetActive(true);
                // activeDuration.gameObject.SetActive(true);

                var durationHeight = musicLaneConfiguration.distancePerTick * (durationInTicks - musicLaneConfiguration.minDurationInTicksToShow);
                
                inactiveDuration.rectTransform.SetHeight(durationHeight);
                activeDuration.rectTransform.SetHeight(durationHeight);
            }
        }
    }
}