using System;
using System.Collections.Generic;
using Game.Components;
using Gemserk.Utilities.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace GBJAM14.UI
{
    public class Option
    {
        public string name;
        public bool disabled;
        public object userData;
    }
    
    public class UIOptions : MonoBehaviour, ISubmitHandler, ISelectHandler
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
        public SoundEffectAsset confirmFailSoundEffect;
        
        public UnityEvent onOptionSelected;

        [NonSerialized]
        public int selectedOptionIndex;

        public Option selectedOption => uiOptions[selectedOptionIndex].option;
        
        public void ShowOptions(List<Option> options)
        {
            optionSelected = false;
            selectedOptionIndex = -1;
            
            var previousOptions = contentParent.GetComponentsInChildren<UIOption>();
            foreach (var previousOption in previousOptions)
            {
                GameObject.Destroy(previousOption.gameObject);
            }

            uiOptions.Clear();
           

            foreach (var option in options)
            {
                var uiOptionObject = GameObject.Instantiate(uiOptionPrefab, contentParent, 
                    false);
                var uiOption = uiOptionObject.GetComponent<UIOption>();
                uiOption.SetOption(option);

                uiOption.gameObject.AddComponent<SubmitHandlerParentDelegate>();
                
                uiOptions.Add(uiOption);
                // uiEventOption.text.SetText(option);
            }
            
            window.Open();
        }
        
        public void OnSelect(BaseEventData eventData)
        {
            if (uiOptions.Count > 0)
            {
                StartCoroutine(InputEventSystemUtils.DelegateSelectionDelayed(uiOptions[0].gameObject));
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            // get selected option, invoke callback
            var optionsList = contentParent.GetComponentsInChildren<UIOption>();
            for (var i = 0; i < optionsList.Length; i++)
            {
                var option = optionsList[i];
                if (option.selected)
                {
                    if (!option.option.disabled)
                    {
                        selectedOptionIndex = i;
                        // selectedUIOption = option;
                        optionSelected = true;
                        onOptionSelected.Invoke();
                        FindAnyObjectByType<UISoundEffects>().PlaySound(confirmSoundEffect);
                        return;
                    }
                    else
                    {
                        FindAnyObjectByType<UISoundEffects>().PlaySound(confirmFailSoundEffect);
                    }
          
                }
            }
            

        }
    }
}