using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class UnitModelComponent : MonoBehaviour, IGameComponent
    {
        public Animator animator;
        public SpriteRenderer model;

        [NonSerialized]
        public Vector2 lookingDirection = new Vector2(1, 0);

        public bool rotateToDirection = false;
    }
}