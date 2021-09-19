using UnityEngine;

namespace GBJAM.Commons.Transitions
{
    public class Transition : MonoBehaviour
    {
        [SerializeField]
        protected Animator animator;

        [SerializeField]
        protected string openState  = "open";
    
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
