using System;
using System.Collections;
using Game.Components;
using Game.Screens;
using GBJAM14.Systems;
using Gemserk.Utilities.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GBJAM14.UI
{
    public class UIDialog : MonoBehaviour
    {
        public UIWindow window;
        public TextView dialogTextView;

        public Image[] portraits;
        public Image[] indicators;
        
        public float textSpeed = 1f;

        public SoundEffectAsset typeSoundEffect;

        public Vector3 currentPortraitOffset;
        
        [NonSerialized]
        public bool completed;

        [NonSerialized]
        public bool waiting;

        public GameObject waitingButton;
        
        private Coroutine showTextCoroutine;

        private string dialogText = string.Empty;

        private CharacterDialogsDB.DialogData dialogData;

        private int currentText;
        
        private void Awake()
        {
            window.onCloseAction.AddListener(Hide);
            waitingButton.SetActive(false);
        }

        public void ShowDialog(CharacterDialogsDB.DialogData dialogData)
        {
            window.Open();
            this.dialogData = dialogData;
            currentText = 0;
            ShowCurrentDialog();
        }

        private void ShowCurrentDialog()
        {
            foreach (var indicator in indicators)
            {
                indicator.enabled = false;
            }

            foreach (var portrait in portraits)
            {
                portrait.rectTransform.localPosition = Vector3.zero;
            }
            
            var dialogText = dialogData.texts[currentText];
            if (dialogData.characters != null)
            {
                for (int i = 0; i < indicators.Length; i++)
                {
                    if (dialogText.StartsWith($"[{i}]"))
                    {
                        indicators[i].enabled = true;
                        portraits[i].rectTransform.localPosition = currentPortraitOffset;
                    }
                }
                
                for (var i = 0; i < dialogData.characters.Length; i++)
                {
                    dialogText = dialogText.Replace($"[{i}]", dialogData.characters[i]);
                }
            }
            ShowText(dialogText);
        }

        private void ShowText(string text)
        {
            waitingButton.SetActive(false);
            
            completed = false;
            waiting = false;
            
            dialogText = text;
            
            if (showTextCoroutine != null)
            {
                StopCoroutine(showTextCoroutine);
                showTextCoroutine = null;
            }
            
            // ideally show step by step...
            showTextCoroutine = StartCoroutine(ShowTextOverTime(1));
        }
        
        private void AppendText(string text)
        {
            waitingButton.SetActive(false);
            
            // I assume it already started
            completed = false;
            waiting = false;
            
            var currentLength = dialogText.Length;
            
            dialogText += text;
            
            if (showTextCoroutine != null)
            {
                StopCoroutine(showTextCoroutine);
                showTextCoroutine = null;
            }
            
            // ideally show step by step...
            showTextCoroutine = StartCoroutine(ShowTextOverTime(currentLength));
        }

        private void Hide()
        {
            if (showTextCoroutine != null)
            {
                StopCoroutine(showTextCoroutine);
                showTextCoroutine = null;
            }
            
            dialogTextView.SetText(string.Empty);
        }

        public void ForceComplete()
        {
            if (showTextCoroutine != null)
            {
                StopCoroutine(showTextCoroutine);
                showTextCoroutine = null;
            }
            
            dialogTextView.SetText(dialogText);
            completed = true;
            waiting = true;
            
            waitingButton.SetActive(true);
        }

        public void CompleteWaiting()
        {
            waiting = false;
        }

        private IEnumerator ShowTextOverTime(int start)
        {
            var uiSoundEffects = FindAnyObjectByType<UISoundEffects>();
                
            for (var i = start; i <= dialogText.Length; i++)
            {
                dialogTextView.SetText(dialogText.Substring(0, i));
                uiSoundEffects.PlaySound(typeSoundEffect);
                yield return new WaitForSecondsRealtime(textSpeed);
            }
            showTextCoroutine = null;
            completed = true;
            waiting = true;
            
            waitingButton.SetActive(true);
        }

        public bool HasPendingText()
        {
            return currentText + 1 < dialogData.texts.Count;
        }

        public void ShowNext()
        {
            currentText++;
            ShowCurrentDialog();
        }
    }
}
