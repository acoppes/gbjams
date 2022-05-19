using System;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace GBJAM9.Ecs
{
    public struct PositionComponent : IEntityComponent
    {
        public Vector2 value;
    }
    
    public struct LookingDirection : IEntityComponent
    {
        public Vector2 value;
    }
    
    public struct UnitInputComponent : IEntityComponent
    {
        public bool disabled;
        
        public Vector2 movementDirection;
        
        public Vector2 attackDirection;

        public bool attack;

        public bool dash;
    }

    public struct UnitModelComponent : IEntityComponent
    {
        public GameObject prefab;
        public GameObject instance;
    }
    
    public struct UnitMovementComponent : IEntityComponent
    {
        public float speed;

        public Vector2 perspective;

        public Vector2 currentVelocity;

        public Vector2 movingDirection;

        public static UnitMovementComponent Default => new()
        {
            perspective = new Vector2(1.0f, 0.75f),
            movingDirection = new Vector2(0, 0)
        };
    }

    public struct UnitStateComponent : IEntityComponent
    {
        public bool walking;
    }

    public struct AnimatorComponent : IEntityComponent
    {
        public Animator animator;
    }
}