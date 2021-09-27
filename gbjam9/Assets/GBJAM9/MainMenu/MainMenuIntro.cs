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

        public bool visible = true;

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
