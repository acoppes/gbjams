using System;
using System.Collections.Generic;
using Game.Components;
using GBJAM13.Data;
using Gemserk.Utilities.UI;
using MyBox;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace GBJAM13.UI
{
    public class UIOptions : MonoBehaviour, ISubmitHandler
    {
        public UIWindow window;

        public RectTransform contentParent;
        
        [FormerlySerializedAs("uiEventOptionPrefab")] 
        public GameObject uiOptionPrefab;
        
        [NonSerialized]
        public bool optionSelected;

        private readonly List<UIOption> uiOptions = new List<UIOption>();
        
        // public InputActionReference upAction;
        // public InputActionReference downAction;
        // public InputActionReference selectAction;

        public SoundEffectAsset confirmSoundEffect;
        
        public UnityEvent onOptionSelected;

        public int selectedOption;
        
        public void ShowOptions(List<string> options)
        {
            optionSelected = false;
            selectedOption = -1;
            
            var previousOptions = contentParent.GetComponentsInChildren<UIOption>();
            foreach (var previousOption in previousOptions)
            {
                GameObject.Destroy(previousOption.gameObject);
            }

            uiOptions.Clear();
           

            foreach (var option in options)
            {
                var uiEventOptionGameObject = GameObject.Instantiate(uiOptionPrefab, contentParent, 
                    false);
                var uiEventOption = uiEventOptionGameObject.GetComponent<UIOption>();
                uiEventOption.SetOption(option);
                
                uiOptions.Add(uiEventOption);
                // uiEventOption.text.SetText(option);
            }
            
            window.Open();
            
            if (uiOptions.Count > 0)
            {
                EventSystem.current.SetSelectedGameObject(uiOptions[0].gameObject);
            }
        }


        public void OnSubmit(BaseEventData eventData)
        {
            optionSelected = true;
            
            // get selected option, invoke callback
            var optionsList = contentParent.GetComponentsInChildren<UIOption>();
            for (var i = 0; i < optionsList.Length; i++)
            {
                var option = optionsList[i];
                if (option.selected)
                {
                    selectedOption = i;
                    // selectedUIOption = option;
                }
            }

            onOptionSelected.Invoke();
            
            FindAnyObjectByType<UISoundEffects>().PlaySound(confirmSoundEffect);
        }
    }
}