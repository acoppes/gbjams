using UnityEngine;
using UnityEngine.UI;

namespace GBJAM12
{
    public class MusicLaneUI : MonoBehaviour
    {
        public MusicLane musicLane;
        
        public Image inactiveImage;
        public Image activeImage;

        public void Update()
        {
            inactiveImage.enabled = !musicLane.buttonPressed;
            activeImage.enabled = musicLane.buttonPressed;
        }
    }
}