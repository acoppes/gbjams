using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;

namespace GBJAM10.Ecs
{
    public class TerrainCollisionSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var terrainCollisionComponents = world.GetComponents<TerrainCollisionComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            var colliderComponents = world.GetComponents<ColliderComponent>();
            
            foreach (var entity in world.GetFilter<TerrainCollisionComponent>()
                         .Inc<PositionComponent>()
                         .Inc<ColliderComponent>()
                         .End())
            {
                ref var terrainCollisionComponent = ref terrainCollisionComponents.Get(entity);
                ref var positionComponent = ref positionComponents.Get(entity);
                var colliderComponent = colliderComponents.Get(entity);

                if (colliderComponent.collisionCount > 0)
                {
                    positionComponent.value = terrainCollisionComponent.lastValidPosition;
                }
                else
                {
                    terrainCollisionComponent.lastValidPosition = positionComponent.value;
                }
            }
        }
    }
}