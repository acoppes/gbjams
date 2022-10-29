using System.Collections.Generic;
using GBJAM.Commons;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace Beatemup.Ecs
{
    public struct LookingDirectionIndicator : IEntityComponent
    {
        public GameObject instance;
    }

    public struct PlayerInputComponent : IEntityComponent
    {
        public bool disabled;
        public int playerInput;
    }
    
    public struct ControlComponent : IEntityComponent
    {
        public Vector2 direction;
    }

    public struct UnitModelComponent : IEntityComponent
    {
        public enum Visiblity
        {
            Visible = 0,
            Hidden = 1
        }
        
        public GameObject prefab;
        public GameObject instance;

        public Transform subModel;

        public bool rotateToDirection;

        public Visiblity visiblity;

        public bool IsVisible => visiblity == Visiblity.Visible;
    }
    
    public struct UnitMovementComponent : IEntityComponent
    {
        public bool disabled;
        
        public float speed;
        public float extraSpeed;

        public Vector2 currentVelocity;

        public Vector2 movingDirection;
        
        public float totalSpeed => speed + extraSpeed;
    }

    public struct JumpComponent : IEntityComponent
    {
        public float y;
    }

    public struct AutoDestroyOutsideCamera : IEntityComponent
    {
        
    }
    
    public struct KeepInsideCameraComponent : IEntityComponent
    {
        
    }
    
    public struct StateTriggers
    {
        public bool hit;
    }
    
    public struct UnitStateComponent : IEntityComponent
    {
        public bool walking;

        public StateTriggers stateTriggers;

        public bool disableAutoUpdate;
    }

    public struct AnimatorComponent : IEntityComponent
    {
        public Animator animator;
    }

    public class TargetExtra
    {
        public Vector2 lookingDirection;
        public bool isAlive;
    }

    public interface ITargetEffect
    {
        
    }

    public class DamageTargetEffect : ITargetEffect
    {
        public float damage;
    }
    
    public struct TargetEffectsComponent : IEntityComponent
    {
        public List<ITargetEffect> targetEffects;
    }

    // public struct PhysicsBodyComponent : IEntityComponent
    // {
    //     // add stuff to initialize the body
    //     public Rigidbody2D bodyInstance;
    // }

    public struct TerrainCollisionComponent : IEntityComponent
    {
        public Vector2 lastValidPosition;
    }

    public struct UnitTypeComponent : IEntityComponent
    {
        public int type;
    }

    public struct SpecialWeaponProviderComponent : IEntityComponent
    {
        public int special;
    }

    public struct SpecialWeaponComponent : IEntityComponent
    {
        public int special;
    }
}