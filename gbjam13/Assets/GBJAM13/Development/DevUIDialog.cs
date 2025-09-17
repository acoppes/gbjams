using GBJAM13.UI;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

namespace GBJAM13.Development
{
    public class DevUIDialog : MonoBehaviour
    {
        [TextArea(2, 5)]
        public string text;

        [FormerlySerializedAs("dialogUI")] 
        public UIDialog uiDialog;

        [ButtonMethod]
        public void ShowText()
        {
            uiDialog.ShowText(text);
        }

        [ButtonMethod]
        public void ForceComplete()
        {
            uiDialog.ForceComplete();
        }
    }
}