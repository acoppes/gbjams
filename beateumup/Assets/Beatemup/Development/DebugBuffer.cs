using System;
using System.Collections.Generic;
using Beatemup.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Beatemup.Development
{
    public class DebugBuffer : MonoBehaviour
    {
        [SerializeField]
        private GameObject actionPrefab;

        [SerializeField]
        private Color historyColor;

        [SerializeField]
        private List<Sprite> actionSprites = new ();
        
        private readonly List<Image> inputActionImages = new ();

        private bool isHistory;
        
        public void Start()
        {
            for (var i = 0; i < ControlComponent.MaxBufferCount; i++)
            {
                var actionInstance = GameObject.Instantiate(actionPrefab, transform);
                actionInstance.SetActive(false);
                inputActionImages.Add(actionInstance.GetComponent<Image>());
            }
        }

        public void ConvertToHistory()
        {
            foreach (var inputAction in inputActionImages)
            {
                inputAction.color = historyColor;
            }

            isHistory = true;
        }

        public void UpdateBuffer(ControlComponent controlComponent)
        {
            if (isHistory)
            {
                return;
            }
            
            for (var i = 0; i < inputActionImages.Count; i++)
            {
                var inputActionImage = inputActionImages[i];
                
                inputActionImage.gameObject.SetActive(false);
                
                if (i >= controlComponent.buffer.Count)
                {
                    continue;
                }
                
                inputActionImage.gameObject.SetActive(true);
                inputActionImage.sprite = 
                    actionSprites.Find(s => s.name.Equals(controlComponent.buffer[i], StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}