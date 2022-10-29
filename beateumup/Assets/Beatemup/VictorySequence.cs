using System;
using UnityEngine;

namespace Beatemup
{
    public class VictorySequence : MonoBehaviour
    {
        [NonSerialized]
        public bool done;
        
        [NonSerialized]
        public bool completed;

        public Animator animator;
        
        public static readonly int doneStateHash = Animator.StringToHash("Done");
        public static readonly int completedStateHash = Animator.StringToHash("Completed");
        
        public static readonly int restartStateHash = Animator.StringToHash("Restart");
        public static readonly int completeStateHash = Animator.StringToHash("Complete");

        public void Restart()
        {
            done = false;
            completed = false;
            
            animator.SetTrigger(restartStateHash);
        }
        
        public void Complete()
        {
            animator.SetTrigger(completeStateHash);
        }

        private void LateUpdate()
        {
            done = animator.GetCurrentAnimatorStateInfo(0).shortNameHash == doneStateHash;
            completed = animator.GetCurrentAnimatorStateInfo(0).shortNameHash == completedStateHash;
        }
    }
}