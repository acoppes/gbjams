using System;
using System.Collections.Generic;
using System.Linq;
using GBJAM.Commons;
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
    }

    public struct UnitModelComponent : IEntityComponent
    {
        public GameObject prefab;
        public GameObject instance;
    }
    
    public struct UnitMovementComponent : IEntityComponent
    {
        public bool disabled;
        
        public float speed;

        public float extraSpeed;

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
        public bool dashing;

        public bool chargeAttack1;
        public bool attacking1;

        public bool chargeAttack2;
        public bool attacking2;
    }

    public class Ability
    {
        public string name;
        
        public float duration;
        
        public float cooldownTotal;
        public float cooldownCurrent;
        
        public float runningTime;
        
        public bool isReady => cooldownCurrent > cooldownTotal && !isRunning;
        public bool isComplete => runningTime > duration;

        public bool isRunning;

        public void StartRunning()
        {
            runningTime = 0;
            isRunning = true;
        }

        public void Complete()
        {
            cooldownCurrent = 0;
            isRunning = false;
        }
    }
    
    public struct AbilitiesComponent : IEntityComponent
    {
        public List<Ability> abilities;

        public Ability Get(string name)
        {
            return abilities.FirstOrDefault(a => a.name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }

    public struct AnimatorComponent : IEntityComponent
    {
        public Animator animator;
    }

    public struct PlayerComponent : IEntityComponent
    {
        public int player;
    }

    public struct Target
    {
        public int entity;
        public int player;
        public Vector2 position;
    }

    // public struct TargetPosition
    // {
    //     public Target target;
    // }

    public struct TargetComponent : IEntityComponent
    {
        public Target target;
    }
    
}