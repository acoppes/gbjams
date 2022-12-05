using UnityEngine;

namespace Beatemup.Screens
{
    public class MainMenuButton : MonoBehaviour
    {
        public GameObject selectionCircle;
        
        public void OnButtonSelected()
        {
            selectionCircle.SetActive(true);
        }

        public void OnButtonDeselected()
        {
            selectionCircle.SetActive(false);
        }
    }
}
