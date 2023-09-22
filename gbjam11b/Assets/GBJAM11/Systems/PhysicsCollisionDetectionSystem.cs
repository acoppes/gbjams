using Game.Components;
using Game.Utilities;
using GBJAM11.Controllers;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;

namespace GBJAM11.Systems
{
    public class PhysicsCollisionDetectionSystem : BaseSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<PhysicsComponent>(entity))
            {
                ref var physics = ref entity.Get<PhysicsComponent>();
                
                if (physics.collisionsEventsDelegate != null)
                {
                    physics.collisionsEventsDelegate.onCollisionEnter += OnEntityCollisionEnter;
                }
            }
        }
        
        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (world.HasComponent<PhysicsComponent>(entity))
            {
                ref var physics = ref entity.Get<PhysicsComponent>();
                
                if (physics.collisionsEventsDelegate != null)
                {
                    physics.collisionsEventsDelegate.onCollisionEnter -= OnEntityCollisionEnter;
                }
            }
        }
        
        // This is gonna happen during the Physics Update
        private static void OnEntityCollisionEnter(World world, Entity entity, IEntityCollisionDelegate.EntityCollision entityCollision)
        {
            if (!entity.Has<ControllerComponent>())
                return;
            
            var controllers = entity.Get<ControllerComponent>();
            foreach (var controller in controllers.controllers)
            {
                if (controller is IEntityCollisionEvent onEvent)
                {
                    onEvent.OnEntityCollision(world, entity, entityCollision);
                }
            }
        }
    }
}