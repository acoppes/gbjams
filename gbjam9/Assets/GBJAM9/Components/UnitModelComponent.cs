using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class UnitModelComponent : MonoBehaviour, IEntityComponent
    {
        public Animator animator;
        public SpriteRenderer model;

        public Transform optionalStartLookAt;

        [NonSerialized]
        public Vector2 lookingDirection = new Vector2(1, 0);

        public bool rotateToDirection;

        public bool verticalFlip;

        private void Awake()
        {
            if (animator == null)
                animator = GetComponent<Animator>();

            if (animator != null)
            {
                animator.logWarnings = false;
            }
        }
    }
}