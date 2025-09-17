using System;
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

        [NonSerialized]
        public bool completed;
        
        private Coroutine showTextCoroutine;

        private string dialogText = string.Empty;

        private void Awake()
        {
            window.onCloseAction.AddListener(Hide);
        }

        public void ShowText(string text)
        {
            completed = false;
            
            window.Open();
            
            dialogText = text;
            
            if (showTextCoroutine != null)
            {
                StopCoroutine(showTextCoroutine);
            }
            
            // ideally show step by step...
            showTextCoroutine = StartCoroutine(ShowTextOverTime(1));
        }
        
        public void AppendText(string text)
        {
            // I assume it already started
            completed = false;
            
            var currentLength = dialogText.Length;
            
            dialogText += text;
            
            if (showTextCoroutine != null)
            {
                StopCoroutine(showTextCoroutine);
            }
            
            // ideally show step by step...
            showTextCoroutine = StartCoroutine(ShowTextOverTime(currentLength));
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
            completed = true;
        }

        private IEnumerator ShowTextOverTime(int start)
        {
            for (var i = start; i <= dialogText.Length; i++)
            {
                dialogTextView.SetText(dialogText.Substring(0, i));
                yield return new WaitForSeconds(textSpeed);
            }
            showTextCoroutine = null;
            completed = true;
        }
    }
}
