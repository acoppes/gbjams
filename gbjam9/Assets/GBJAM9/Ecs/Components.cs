using System;
using System.Collections.Generic;
using System.Linq;
using GBJAM.Commons;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace GBJAM9.Ecs
{
    public struct LookingDirectionIndicator : IEntityComponent
    {
        public GameObject instance;
    }

    public struct PlayerInputComponent : IEntityComponent
    {
        public bool disabled;
        public GameboyButtonKeyMapAsset keyMap;
    }
    
    public struct UnitControlComponent : IEntityComponent
    {
        public Vector2 direction;
        
        // public Vector2 attackDirection;

        public bool mainAction;

        public bool secondaryAction;
        public bool locked;
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
    }

    public struct StateTriggers
    {
        public bool hit;
    }

    public struct UnitStateComponent : IEntityComponent
    {
        public bool walking;
        public bool dashing;
        public bool isDeath;

        public bool chargeAttack1;
        public bool attacking1;

        public bool chargeAttack2;
        public bool attacking2;

        public StateTriggers stateTriggers;
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

    public struct Damage
    {
        public float value;
    }

    public struct HealthComponent : IEntityComponent
    {
        public float current;
        public float total;

        public bool deathRequest;

        public bool isDeath => current <= 0f;

        public List<Damage> pendingDamages;

        public bool autoDestroyOnDeath;
    }

}