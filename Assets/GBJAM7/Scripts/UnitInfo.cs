using UnityEngine;

namespace GBJAM7.Scripts
{
    public class UnitInfo : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;
        
        public void Preview(Unit unit)
        {
            canvasGroup.alpha = 1;
        }

        public void Hide()
        {
            canvasGroup.alpha = 0;
        }
    }
}
