using System;
using System.Collections.Generic;
using System.Linq;
using GBJAM.Commons;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace GBJAM9.Ecs
{
    public struct LookingDirection : IEntityComponent
    {
        public Vector2 value;
        public bool disableIndicator;
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
        public bool locked;
    }

    public struct UnitModelComponent : IEntityComponent
    {
        public GameObject prefab;
        public GameObject instance;

        public bool rotateToDirection;
    }
    
    public struct UnitMovementComponent : IEntityComponent
    {
        public bool disabled;
        
        public float speed;

        public float extraSpeed;

        public Vector2 currentVelocity;

        public Vector2 movingDirection;
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
        public enum StartType
        {
            Loaded = 0,
            Unloaded = 1
        }
        
        public string name;
        
        public float duration;
        
        public float cooldownTotal;
        public float cooldownCurrent;
        
        public float runningTime;
        
        public bool isReady => cooldownCurrent > cooldownTotal && !isRunning;
        public bool isComplete;

        public bool isRunning;

        public Vector2 position;
        public Vector2 direction;

        public IEntityDefinition projectileDefinition;
        
        public Ability.StartType startType;

        public void StartRunning()
        {
            runningTime = 0;
            isRunning = true;
            // isComplete = false;
        }

        public void Stop()
        {
            cooldownCurrent = 0;
            isRunning = false;
            // isComplete = false;
        }

        public void Cancel()
        {
            isRunning = false;
            // isComplete = false;
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

    public struct ProjectileComponent : IEntityComponent
    {
        public Vector2 startPosition;
        public Vector2 startDirection;

        public bool started;
    }

    public class TargetExtra
    {
        public Vector2 lookingDirection;
    }

}