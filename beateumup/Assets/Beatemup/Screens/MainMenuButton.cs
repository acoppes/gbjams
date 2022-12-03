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
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
