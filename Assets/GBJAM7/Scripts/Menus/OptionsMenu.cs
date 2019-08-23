using System;
using System.Collections;
using System.Collections.Generic;
using GBJAM7.Scripts.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace GBJAM7.Scripts
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        public GameboyButtonKeyMapAsset keyMapAsset;

        [SerializeField]
        private GameObject menuOptionPrefab;
        
        private List<MenuOption> menuOptions = new List<MenuOption>();

        private int currentOptionIndex = 0;
        
        // internal register for selected option and then call game controls
        private bool updateLogic;

        [SerializeField]
        private Transform menuOptionsContainer;

        private Action<int, Option> _optionSelectedCallback;
        private Action _cancelMenu;

        public string title;

        public GameObject titleObject;
        public Text titleText;
        
        public void Show(List<Option> options, Action<int, Option> optionSelectedCallback, Action cancelMenu)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            _cancelMenu = cancelMenu;
            _optionSelectedCallback = optionSelectedCallback;
            
            // clear childs?

            foreach (var option in options)
            {
                var menuOptionObject = Instantiate(menuOptionPrefab, menuOptionsContainer);
                var menuOption = menuOptionObject.GetComponentInChildren<MenuOption>();
                menuOption.option = option;
            }

            menuOptions.Clear();
            GetComponentsInChildren(menuOptions);
            
            StartCoroutine(DelayActions());
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            
            var toDestroy = new List<Transform>();
            for (var i = 0; i < menuOptionsContainer.childCount; i++)
            {
                var t = menuOptionsContainer.GetChild(i);
                if (t.GetComponentInChildren<MenuOption>() != null)
                     toDestroy.Add(menuOptionsContainer.GetChild(i));
            }
            
            //menuOptionsContainer.DetachChildren();
            toDestroy.ForEach(t => Destroy(t.gameObject));
        }

        private IEnumerator DelayActions()
        {
            updateLogic = false;
            yield return null;
            updateLogic = true;
        }

        public void Update()
        {
            if (_canvasGroup.alpha <= 0.01f || !updateLogic)
            {
                return;
            }

//            if (!gameControls.keyReady)
//                return;
            
            if (keyMapAsset.upPressed)
            {
                currentOptionIndex--;
                if (currentOptionIndex < 0)
                    currentOptionIndex = menuOptions.Count - 1;
                // move to previous option
            }

            if (keyMapAsset.downPressed)
            {
                currentOptionIndex++;
                if (currentOptionIndex >= menuOptions.Count)
                {
                    currentOptionIndex = 0;
                }
            }

            if (keyMapAsset.button1Pressed)
            {
                // execute action in game controls!
                _optionSelectedCallback(currentOptionIndex, menuOptions[currentOptionIndex].option);
                // Hide();
            }

            if (keyMapAsset.button2Pressed)
            {
                // hide menu 
                _cancelMenu();
                // Hide();
            }
        }

        private void LateUpdate()
        {
            titleText.text = title;
            titleObject.SetActive(!string.IsNullOrEmpty(title));

            for (var i = 0; i < menuOptions.Count; i++)
            {
                menuOptions[i].selected = i == currentOptionIndex;
            }
        }
    }
}