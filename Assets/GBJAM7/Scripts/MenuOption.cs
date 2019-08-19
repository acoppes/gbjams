using UnityEngine;
using UnityEngine.UI;

namespace GBJAM7.Scripts
{
    public class MenuOption : MonoBehaviour
    {
        public Image selector;
        
        public bool selected;

        private void LateUpdate()
        {
            selector.enabled = selected;
        }
    }
}
