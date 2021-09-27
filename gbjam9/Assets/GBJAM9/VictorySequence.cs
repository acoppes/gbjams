using System;
using UnityEngine;

namespace GBJAM9
{
    public class VictorySequence : MonoBehaviour
    {
        [NonSerialized]
        public bool done;

        public Animator animator;
        
        public static readonly int doneStateHash = Animator.StringToHash("Done");
        public static readonly int restartStateHash = Animator.StringToHash("Restart");

        public void Restart()
        {
            done = false;
            animator.SetTrigger(restartStateHash);
        }

        private void LateUpdate()
        {
            done = animator.GetCurrentAnimatorStateInfo(0).shortNameHash == doneStateHash;
        }
    }
}