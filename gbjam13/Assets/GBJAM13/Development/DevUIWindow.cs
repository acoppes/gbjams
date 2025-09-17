using Gemserk.Utilities.UI;
using MyBox;
using UnityEngine;

namespace GBJAM13.Development
{
    public class DevUIWindow : MonoBehaviour
    {
        public UIWindow window;

        [ButtonMethod]
        public void Show()
        {
            window.Open();
        }
        
        [ButtonMethod]
        public void Hide()
        {
            window.Close();
        }
    }
}