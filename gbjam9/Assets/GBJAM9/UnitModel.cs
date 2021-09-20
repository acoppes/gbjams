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

        [NonSerialized]
        public Vector2 velocity;

        private void LateUpdate()
        {
            if (animator != null)
            {
                animator.SetBool(walkingStateHash, velocity.SqrMagnitude() > 0);
            }

            if (model != null && Mathf.Abs(velocity.x) > 0)
            {
                model.flipX = velocity.x < 0;
            }
        }
    }
}