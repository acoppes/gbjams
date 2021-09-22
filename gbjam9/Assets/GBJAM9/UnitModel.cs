using System;
using UnityEngine;

namespace GBJAM9
{
    public class UnitModel : MonoBehaviour
    {
        [SerializeField]
        protected Animator animator;

        [SerializeField]
        protected SpriteRenderer model;

        private readonly int walkingStateHash = Animator.StringToHash("walking");

        private readonly int dashingStateHash = Animator.StringToHash("dashing");

        private readonly int kunaiAttackStateHash = Animator.StringToHash("kunai_attack");
        
        private readonly int swordAttackStateHash = Animator.StringToHash("sword_attack");
        
        private readonly int hitStateHash = Animator.StringToHash("hit");

        [NonSerialized]
        public Vector2 lookingDirection = new Vector2(1, 0);

        public bool rotateToDirection = false;
        
        [NonSerialized]
        public Vector2 velocity;

        [NonSerialized]
        public UnitState unitState;

        private void LateUpdate()
        {
            if (animator != null)
            {
                animator.SetBool(walkingStateHash, unitState.walking);
                animator.SetBool(kunaiAttackStateHash, unitState.kunaiAttacking);
                animator.SetBool(dashingStateHash, unitState.dashing);
                // animator.SetBool(swordAttackStateHash, unitState.state == UnitState.State.SwordAttack);
                animator.SetBool(hitStateHash, unitState.hit);
            }

            if (!rotateToDirection)
            {
                if (Mathf.Abs(lookingDirection.x) > 0)
                {
                    model.flipX = lookingDirection.x < 0;
                }
            }
            else
            {
                var angle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg;
                model.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }
}