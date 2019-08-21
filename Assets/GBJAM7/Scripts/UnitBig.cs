using UnityEngine;

namespace GBJAM7.Scripts
{
    public class UnitBig : MonoBehaviour
    {
        public Animator animator;
        
        public void StartAttacking()
        {
            animator.SetBool("Attacking", true);
        }

        public void StopAttacking()
        {
            animator.SetBool("Attacking", false);
        }
    }
}