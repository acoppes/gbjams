using System;
using GBJAM13.UI;
using MyBox;
using UnityEngine;

namespace GBJAM13.Development
{
    public class DevDialogUI : MonoBehaviour
    {
        [TextArea(2, 5)]
        public string text;

        public DialogUI dialogUI;

        private void Start()
        {
            ShowText();
        }

        [ButtonMethod]
        public void ShowText()
        {
            dialogUI.ShowText(text);
        }

        [ButtonMethod]
        public void ForceComplete()
        {
            dialogUI.ForceComplete();
        }
    }
}