using UnityEngine;

namespace GBJAM7.Scripts
{
    public class UnitAnimator : MonoBehaviour
    {
        public Animator animator;
        public Unit unit;
        public SpriteRenderer spriteRenderer;

        private void LateUpdate()
        {
            animator.SetBool("captured", unit.player != -1);
            spriteRenderer.flipX = unit.player == 1;
        }
    }
}