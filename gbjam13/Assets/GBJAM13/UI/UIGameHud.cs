using System;
using System.Collections.Generic;
using Gemserk.Utilities.UI;
using UnityEngine;

namespace GBJAM13.UI
{
    public class UIGameHud : MonoBehaviour
    {
        public UIWindow window;

        [NonSerialized]
        public List<UIResource> uiResources = new List<UIResource>();
        
        private void Awake()
        {
            window.onOpenAction.AddListener(OnWindowOpen);
            GetComponentsInChildren(uiResources);
        }

        private void OnWindowOpen()
        {
            // update resources from savegame?
            var saveGame = GameParameters.saveGame;

            foreach (var uiResource in uiResources)
            {
                uiResource.SetValue(saveGame.resources[uiResource.resourceType.value]);
            }
        }
    }
}