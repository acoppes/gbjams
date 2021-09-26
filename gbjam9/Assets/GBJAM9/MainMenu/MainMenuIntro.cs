using System;
using UnityEngine;

namespace GBJAM7.Scripts.MainMenu
{
    public class MainMenuIntro : MonoBehaviour
    {
        public Animator animator;
        
        [NonSerialized]
        public bool completed;

        public GameObject pressStartObject;

        public Font font;

        public bool visible = true;

        private void Awake()
        {
            if (font != null)
            {
                var texture = font.material.mainTexture;
                texture.filterMode = FilterMode.Point;
            }
        }

        public void OnCompleted()
        {
            completed = true;
        }

        public void ForceComplete()
        {
            completed = true;
            animator.Play("Idle", -1, 0);
        }

        public void HideStart()
        {
            pressStartObject.SetActive(false);
        }

        public void ShowStart()
        {
            pressStartObject.SetActive(true);
        }

        private void LateUpdate()
        {
            animator.SetBool("visible", visible);
        }
    }
}
