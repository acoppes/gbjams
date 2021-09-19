using UnityEngine;

namespace GBJAM.Commons.Transitions
{
    public class Transition : MonoBehaviour
    {
        [SerializeField]
        protected Animator animator;

        [SerializeField]
        protected string openState  = "open";

        private readonly int openStateHash = Animator.StringToHash("Open");
        private readonly int closeStateHash = Animator.StringToHash("Close");
        
        public bool isOpen => animator.GetCurrentAnimatorStateInfo(0).shortNameHash == openStateHash;
        public bool isClosed => animator.GetCurrentAnimatorStateInfo(0).shortNameHash == closeStateHash;
    
        public void Open()
        {
            animator.SetBool(openState, true);
        }

        public void Close()
        {
            animator.SetBool(openState, false);
        }

    }
}
