using System;
using System.Collections;
using Game.Components;
using Game.Screens;
using Gemserk.Utilities.UI;
using MyBox;
using UnityEngine;

namespace GBJAM13.UI
{
    public class UIDialog : MonoBehaviour
    {
        public UIWindow window;
        public TextView dialogTextView;

        public float textSpeed = 1f;

        public AudioSource audioSource;
        public SoundEffectAsset typeSoundEffect;

        [NonSerialized]
        public bool completed;

        [NonSerialized]
        public bool waiting;

        public GameObject waitingButton;
        
        private Coroutine showTextCoroutine;

        private string dialogText = string.Empty;
        
        private void Awake()
        {
            window.onCloseAction.AddListener(Hide);
            waitingButton.SetActive(false);
        }

        public void ShowText(string text)
        {
            waitingButton.SetActive(false);
            
            completed = false;
            waiting = false;
            
            window.Open();
            
            dialogText = text;
            
            if (showTextCoroutine != null)
            {
                StopCoroutine(showTextCoroutine);
                showTextCoroutine = null;
            }
            
            // ideally show step by step...
            showTextCoroutine = StartCoroutine(ShowTextOverTime(1));
        }
        
        public void AppendText(string text)
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
            for (var i = start; i <= dialogText.Length; i++)
            {
                dialogTextView.SetText(dialogText.Substring(0, i));
                PlaySound(typeSoundEffect);
                yield return new WaitForSeconds(textSpeed);
            }
            showTextCoroutine = null;
            completed = true;
            waiting = true;
            
            waitingButton.SetActive(true);
        }
        
        private void PlaySound(SoundEffectAsset asset)
        {
            if (!audioSource)
                return;
            
            audioSource.pitch = asset.randomPitch.RandomInRange();
            audioSource.clip = asset.clips.GetRandom();
            audioSource.volume = asset.volume;
            audioSource.outputAudioMixerGroup = asset.mixerGroup;
            audioSource.PlayOneShot(asset.clips.GetRandom());
        }
    }
}
