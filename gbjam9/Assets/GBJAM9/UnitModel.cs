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

        private readonly int dashingStateHash = Animator.StringToHash("dash");

        private readonly int hurtStateHash = Animator.StringToHash("hurt");
        
        private readonly int hitStateHash = Animator.StringToHash("hit");

        [NonSerialized]
        public Vector2 lookingDirection = new Vector2(1, 0);

        public bool rotateToDirection = false;
        
        [NonSerialized]
        public Vector2 velocity;

        // [NonSerialized]
        // public UnitState unitState;

        public bool hit;
        
        private void LateUpdate()
        {
            if (animator != null)
            {
                animator.SetBool(walkingStateHash, velocity.SqrMagnitude() > 0);
                //animator.SetBool(dashingStateHash, dashingCurrentTime > 0);
                
                animator.SetBool(hitStateHash, hit);
                hit = false;
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