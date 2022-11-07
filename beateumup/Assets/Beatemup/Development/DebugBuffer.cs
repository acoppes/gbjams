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
        private List<Sprite> actionSprites = new ();
        
        private readonly List<Image> inputActionImages = new ();
        
        public void Start()
        {
            for (var i = 0; i < ControlComponent.MaxBufferCount; i++)
            {
                var actionInstance = GameObject.Instantiate(actionPrefab, transform);
                actionInstance.SetActive(false);
                inputActionImages.Add(actionInstance.GetComponent<Image>());
            }
        }

        public void UpdateBuffer(ControlComponent controlComponent)
        {
            foreach (var inputAction in inputActionImages)
            {
                inputAction.gameObject.SetActive(false);
            }

            for (var i = 0; i < inputActionImages.Count; i++)
            {
                if (i >= controlComponent.buffer.Count)
                {
                    break;
                }
                
                inputActionImages[i].gameObject.SetActive(true);
                inputActionImages[i].sprite = 
                    actionSprites.Find(s => s.name.Equals(controlComponent.buffer[i], StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}