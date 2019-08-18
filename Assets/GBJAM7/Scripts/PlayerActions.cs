using UnityEngine;

namespace GBJAM7.Scripts
{
    public class PlayerActions : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
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
