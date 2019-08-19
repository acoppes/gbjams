using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBJAM7.Scripts
{
    public class PlayerActions : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        public GameControls gameControls;
        
        private List<MenuOption> options = new List<MenuOption>();

        private int currentOptionIndex = 0;
        
        // internal register for selected option and then call game controls
        private bool updateLogic;
        
        public void Show()
        {
            _canvasGroup.alpha = 1;
            GetComponentsInChildren(options);

            StartCoroutine(DelayActions());
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
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

            if (gameControls.upPressed)
            {
                currentOptionIndex--;
                if (currentOptionIndex < 0)
                    currentOptionIndex = options.Count - 1;
                // move to previous option
            }

            if (gameControls.downPressed)
            {
                currentOptionIndex++;
                if (currentOptionIndex >= options.Count)
                {
                    currentOptionIndex = 0;
                }
            }

            if (gameControls.button1Pressed)
            {
                // execute action in game controls!
            }

            if (gameControls.button2Pressed)
            {
                // hide menu 
                gameControls.CancelMenuAction();
                Hide();
            }
        }

        private void LateUpdate()
        {
            for (var i = 0; i < options.Count; i++)
            {
                options[i].selected = i == currentOptionIndex;
            }
        }
    }
}
