using System.Collections;
using Game.Screens;
using UnityEngine;

namespace GBJAM13.UI
{
    public class DialogUI : MonoBehaviour
    {
        public TextView dialogTextView;

        public float textSpeed = 1f;

        private Coroutine showTextCoroutine;

        private string dialogText;

        public void ShowText(string text)
        {
            dialogText = text;
            
            if (showTextCoroutine != null)
            {
                StopCoroutine(showTextCoroutine);
            }
            
            // ideally show step by step...
            showTextCoroutine = StartCoroutine(ShowTextOverTime());
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
