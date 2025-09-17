using System.Collections;
using Game.Screens;
using Gemserk.Utilities.UI;
using UnityEngine;

namespace GBJAM13.UI
{
    public class UIDialog : MonoBehaviour
    {
        public UIWindow window;
        public TextView dialogTextView;

        public float textSpeed = 1f;
        
        private Coroutine showTextCoroutine;

        private string dialogText;

        private void Awake()
        {
            window.onCloseAction.AddListener(Hide);
        }

        public void ShowText(string text)
        {
            window.Open();
            
            dialogText = text;
            
            if (showTextCoroutine != null)
            {
                StopCoroutine(showTextCoroutine);
            }
            
            // ideally show step by step...
            showTextCoroutine = StartCoroutine(ShowTextOverTime());
        }

        private void Hide()
        {
            if (showTextCoroutine != null)
            {
                StopCoroutine(showTextCoroutine);
            }
            dialogTextView.SetText(string.Empty);
        }

        public void ForceComplete()
        {
            if (showTextCoroutine != null)
            {
                StopCoroutine(showTextCoroutine);
            }
            dialogTextView.SetText(dialogText);
        }

        private IEnumerator ShowTextOverTime()
        {
            for (var i = 0; i <= dialogText.Length; i++)
            {
                dialogTextView.SetText(dialogText.Substring(0, i));
                yield return new WaitForSeconds(textSpeed);
            }
            showTextCoroutine = null;
        }
    }
}
