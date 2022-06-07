using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace GBJAM9.Ecs
{
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
    }
}