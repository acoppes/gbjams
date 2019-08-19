using UnityEngine;

namespace GBJAM7.Scripts
{
    public class BuildActions : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        public GameControls gameControls;
        
        // internal register for selected option and then call game controls
        
        public void Show()
        {
            _canvasGroup.alpha = 1;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
        }
    }
}