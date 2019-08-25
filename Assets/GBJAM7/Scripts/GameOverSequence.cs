using System;
using UnityEngine;

namespace GBJAM7.Scripts
{
    public class GameOverSequence : MonoBehaviour
    {
        [NonSerialized]
        public bool completed;

        public Animator animator;

        public void StartSequence()
        {
            animator.SetTrigger("gameOver");
        }

        public void OnComplete()
        {
            completed = true;
        }

        public void ForceComplete()
        {
            // TODO: set animator forced state
            completed = true;
        }
    }
}