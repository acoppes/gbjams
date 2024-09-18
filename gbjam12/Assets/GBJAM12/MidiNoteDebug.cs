using UnityEngine;
using UnityEngine.UI;

namespace GBJAM12
{
    public class MidiNoteDebug : MonoBehaviour
    {
        public string track;
        public int note;

        public Image image;

        public bool isPlaying;

        private void LateUpdate()
        {
            image.enabled = isPlaying;
        }
    }
}