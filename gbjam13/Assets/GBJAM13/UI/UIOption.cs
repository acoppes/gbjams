using System;
using Game.Components;
using Game.Screens;
using MyBox;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GBJAM13.UI
{
    public class UIOption : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public TextView text;
        
        public Image selectedImage;
        public Image notSelectedImage;

        public Color enabledColor;
        public Color disabledColor;
        
        [NonSerialized]
        public bool selected;
        
        public SoundEffectAsset selectedSoundEffect;

        // [NonSerialized]
        // public EventElementData.Option option;

        public Option option;
        
        public void SetOption(Option newOption)
        {
            option = newOption;
            
            // option = eventOption;
            // var number = UnityEngine.Random.Range(1, 4);
            // text.SetText($"{option.description} (-{number} {eventOption.resourceType.name})");
            
            text.SetText(newOption.name);
        }

        private void LateUpdate()
        {
            selectedImage.enabled = selected;
            notSelectedImage.enabled = !selected;
            
            text.color = option.disabled ? disabledColor : enabledColor;
        }

        public void OnSelect(BaseEventData eventData)
        {
            selected = true;
            FindAnyObjectByType<UISoundEffects>().PlaySound(selectedSoundEffect);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            selected = false;
        }
    }
}