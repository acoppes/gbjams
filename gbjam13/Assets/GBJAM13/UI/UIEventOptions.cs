using System;
using System.Collections.Generic;
using Game.Components;
using GBJAM13.Data;
using Gemserk.Utilities.UI;
using MyBox;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GBJAM13.UI
{
    public class UIEventOptions : MonoBehaviour, ISubmitHandler
    {
        public UIWindow window;

        public RectTransform contentParent;
        
        public GameObject uiEventOptionPrefab;
        
        [NonSerialized]
        public bool optionSelected;

        private readonly List<UIEventOption> options = new List<UIEventOption>();
        
        // public InputActionReference upAction;
        // public InputActionReference downAction;
        // public InputActionReference selectAction;

        public SoundEffectAsset confirmSoundEffect;
        
        public UnityEvent onOptionSelected;

        private UIEventOption selectedUIOption;

        public EventElementData.Option selectedOption => selectedUIOption.option;
        
        public void ShowOptions(EventElementData.Option[] eventDataOptions)
        {
            optionSelected = false;
            selectedUIOption = null;
            
            var previousOptions = contentParent.GetComponentsInChildren<UIEventOption>();
            foreach (var previousOption in previousOptions)
            {
                GameObject.Destroy(previousOption.gameObject);
            }

            options.Clear();
           

            foreach (var option in eventDataOptions)
            {
                var uiEventOptionGameObject = GameObject.Instantiate(uiEventOptionPrefab, contentParent, 
                    false);
                var uiEventOption = uiEventOptionGameObject.GetComponent<UIEventOption>();
                uiEventOption.SetOption(option);
                
                options.Add(uiEventOption);
                // uiEventOption.text.SetText(option);
            }
            
            window.Open();
            
            if (options.Count > 0)
            {
                EventSystem.current.SetSelectedGameObject(options[0].gameObject);
            }
        }


        public void OnSubmit(BaseEventData eventData)
        {
            optionSelected = true;
            
            // get selected option, invoke callback
            var optionsList = contentParent.GetComponentsInChildren<UIEventOption>();
            foreach (var option in optionsList)
            {
                if (option.selected)
                {
                    selectedUIOption = option;
                }
            }
            onOptionSelected.Invoke();
            
            FindAnyObjectByType<UISoundEffects>().PlaySound(confirmSoundEffect);
        }
    }
}