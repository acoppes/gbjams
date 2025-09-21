using System;
using System.Collections.Generic;
using Gemserk.Utilities.UI;
using UnityEngine;

namespace GBJAM13.UI
{
    public class UIGameHud : MonoBehaviour
    {
        public UIWindow window;

        private readonly List<UIResource> uiResources = new List<UIResource>();
        
        private void Awake()
        {
            GetComponentsInChildren(uiResources);
        }
        
        private void LateUpdate()
        {
            if (!window.IsOpen())
            {
                return;
            }
            
            var saveGame = GameParameters.saveGame;

            foreach (var uiResource in uiResources)
            {
                uiResource.SetValue(saveGame.resources[uiResource.resourceType.value]);
            }
        }
    }
}