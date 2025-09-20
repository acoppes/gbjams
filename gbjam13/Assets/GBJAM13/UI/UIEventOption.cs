using System;
using Game.Components;
using Game.Screens;
using GBJAM13.Data;
using MyBox;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GBJAM13.UI
{
    public class UIEventOption : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public TextView text;

        public Image selectedImage;
        public Image notSelectedImage;
        
        [NonSerialized]
        public bool selected;
        
        public AudioSource audioSource;
        public SoundEffectAsset selectedSoundEffect;

        [NonSerialized]
        public EventElementData.Option option;
        
        public void SetOption(EventElementData.Option eventOption)
        {
            option = eventOption;
            text.SetText(option.description);
        }

        private void LateUpdate()
        {
            selectedImage.enabled = selected;
            notSelectedImage.enabled = !selected;
        }

        public void OnSelect(BaseEventData eventData)
        {
            selected = true;
            PlaySound(selectedSoundEffect);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            selected = false;
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