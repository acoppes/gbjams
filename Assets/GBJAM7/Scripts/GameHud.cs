using UnityEngine;

namespace GBJAM7.Scripts
{
    public class GameHud : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;
        
        public void Hide()
        {
            canvasGroup.alpha = 0;
        }

        public void Show()
        {
            canvasGroup.alpha = 1;
        }
    }
}