using Game.Screens;
using Gemserk.Utilities.UI;
using UnityEngine;

namespace GBJAM13.UI
{
    public class UIMapSelection : MonoBehaviour
    {
        public UIWindow window;

        public TextView textView;

        public void SetSelectedElementData(string elementName)
        {
            if (string.IsNullOrEmpty(elementName))
            {
                window.Close();
                return;
            }
            
            textView.SetText(elementName);
            window.Open();
        }
    }
}