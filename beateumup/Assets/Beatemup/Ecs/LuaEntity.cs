using System.Collections.Generic;
using System.Linq;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class LuaStatesComponent
    {
        public StatesComponent statesComponent;
    
        public void Enter(string state)
        {
            statesComponent.EnterState(state);
        }
        
        public void Exit(string state)
        {
            statesComponent.ExitState(state);
        }

        public bool this[string stateName] => statesComponent.HasState(stateName);
    }

    public class LuaTarget
    {
        public Vector2 position;
    }

    public class LuaAbility
    {
        // public Ability ability;
        public List<LuaTarget> targets = new List<LuaTarget>();
    }
    
    public class LuaAbilitiesComponent
    {
        public AbilitiesComponent abilitiesComponent;

        public LuaAbility this[string abilityName] => new LuaAbility
        {
            targets = abilitiesComponent.GetTargeting(abilityName)
                .targets.Select(t => new LuaTarget
                {
                    position = t.position
                }).ToList()
        };
    }

    public class LuaEntity
    {
        public Gemserk.Leopotam.Ecs.World world;
        public Entity entity;

        public Vector2 position
        {
            get => world.GetComponent<PositionComponent>(entity).value;
            set
            {
                ref var p = ref world.GetComponent<PositionComponent>(entity);
                p.value = value;
            }
        }

        public float speed
        {
            get => world.GetComponent<UnitMovementComponent>(entity).speed;
            set
            {
                ref var p = ref world.GetComponent<UnitMovementComponent>(entity);
                p.speed = value;
            }
        }
        public UnitMovementComponent movement
        {
            get => world.GetComponent<UnitMovementComponent>(entity);
            set
            {
                ref var p = ref world.GetComponent<UnitMovementComponent>(entity);
                p = value;
            }
        }
        
        public Vector2 controlDirection
        {
            get => world.GetComponent<ControlComponent>(entity).direction;
            set
            {
                ref var controlComponent = ref world.GetComponent<ControlComponent>(entity);
                controlComponent.direction = value;
            }
        }

        public void ResetDirection()
        {
            ref var controlComponent = ref world.GetComponent<ControlComponent>(entity);
            controlComponent.direction = Vector2.zero;
        }
        
        public LuaStatesComponent states => new ()
        {
            statesComponent = world.GetComponent<StatesComponent>(entity)
        };
        
        public LuaAbilitiesComponent abilities => new ()
        {
            abilitiesComponent = world.GetComponent<AbilitiesComponent>(entity)
        };

        public bool HasState(string state)
        {
            if (world.HasComponent<StatesComponent>(entity))
            {
                return world.GetComponent<StatesComponent>(entity).HasState(state);
            }
            return false;
        }
        
        public void EnterState(string state)
        {
            var stateComponent = world.GetComponent<StatesComponent>(entity);
            stateComponent.EnterState(state);
        }
        
        public void ExitState(string state)
        {
            var stateComponent = world.GetComponent<StatesComponent>(entity);
            stateComponent.ExitState(state);
        }
        
        public static LuaEntity operator +(LuaEntity luaEntity, string state)
        {
            luaEntity.world.GetComponent<StatesComponent>(luaEntity.entity).EnterState(state);
            return luaEntity;
        }
        
        public static LuaEntity operator -(LuaEntity luaEntity, string state)
        {
            luaEntity.world.GetComponent<StatesComponent>(luaEntity.entity).ExitState(state);
            return luaEntity;
        }

        // public StatesIndexer states => new StatesIndexer();
    }
}