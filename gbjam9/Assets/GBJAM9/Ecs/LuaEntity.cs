using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace GBJAM9.Ecs
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

        public UnitMovementComponent movement
        {
            get => world.GetComponent<UnitMovementComponent>(entity);
            set
            {
                ref var p = ref world.GetComponent<UnitMovementComponent>(entity);
                p = value;
            }
        }

        public LuaStatesComponent states => new ()
        {
            statesComponent = world.GetComponent<StatesComponent>(entity)
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