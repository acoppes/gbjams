using System;
using UnityEngine;

namespace GBJAM7.Scripts.MainMenu
{
    public class MainMenuIntro : MonoBehaviour
    {
        public Animator animator;
        
        [NonSerialized]
        public bool completed;

        public void OnCompleted()
        {
            completed = true;
        }

        public void ForceComplete()
        {
            completed = true;
            animator.Play("Idle", -1, 0);
        }
    }
}
