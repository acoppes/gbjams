using MyBox;
using UnityEngine;
using UnityEngine.UI;

namespace GBJAM12
{
    public class MusicLaneNoteUI : MonoBehaviour
    {
        public MusicLaneNote note;

        public Image inactiveDuration;
        public Image activeDuration;
        public Image activeNote;
        
        private void Start()
        {
            inactiveDuration.gameObject.SetActive(false);
            activeDuration.gameObject.SetActive(false);
            activeNote.gameObject.SetActive(false);
            
            if (note.durationInTicks >= note.gameConfiguration.minDurationInTicksToShow)
            {
                inactiveDuration.gameObject.SetActive(true);
                // activeDuration.gameObject.SetActive(true);

                var durationHeight = note.gameConfiguration.distancePerTick * (note.durationInTicks - note.gameConfiguration.minDurationInTicksToShow);
                
                inactiveDuration.rectTransform.SetHeight(durationHeight);
                activeDuration.rectTransform.SetHeight(durationHeight);
            }
        }

        private void LateUpdate()
        {
            activeNote.gameObject.SetActive(note.wasActivated);
            
            if (note.activeTicks >= note.gameConfiguration.minDurationInTicksToShow)
            {
                activeDuration.gameObject.SetActive(true);
                var durationHeight = note.gameConfiguration.distancePerTick * (note.activeTicks - note.gameConfiguration.minDurationInTicksToShow);
                activeDuration.rectTransform.SetHeight(durationHeight);
            }
        }
    }
}