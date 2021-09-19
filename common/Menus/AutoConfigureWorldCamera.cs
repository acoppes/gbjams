using UnityEngine;

namespace GBJAM.Commons.Menus
{
    public class AutoConfigureWorldCamera : MonoBehaviour
    {
        [SerializeField]
        protected Canvas canvas;

        private void Awake()
        {
            if (canvas == null)
            {
                canvas = GetComponent<Canvas>();
            }
            
            if (canvas != null)
            {
                canvas.worldCamera = Camera.main;
            }
        }
    }
}