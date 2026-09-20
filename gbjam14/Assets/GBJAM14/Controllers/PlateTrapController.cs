using Game.Components;
using Game.Utilities;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using UnityEngine;

namespace GBJAM14.Controllers
{
    public class PlateTrapController : ControllerBase, IInit, IUpdate
    {
        public Object pressSfxDefinition;
        public Object unpressSfxDefinition;
        
        public void OnInit(World world, Entity entity)
        {
            var physics2DComponent = entity.Get<Physics2dComponent>();
            physics2DComponent.collisionsEventsDelegate.onCollisionEnter += OnEntityCollision;
            physics2DComponent.collisionsEventsDelegate.onCollisionExit += OnEntityCollisionExit;
        }

        private void OnEntityCollision(World world, Entity entity, IEntityCollisionDelegate.EntityCollision entityCollision)
        {
            ref var plates = ref entity.Get<PlatesTrapComponent>();
            plates.pressCount++;
        }
        
        private void OnEntityCollisionExit(World world, Entity entity, IEntityCollisionDelegate.EntityCollision entityCollision)
        {
            ref var plates = ref entity.Get<PlatesTrapComponent>();
            plates.pressCount--;
        }

        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var platesTrap = ref entity.Get<PlatesTrapComponent>();
        
            ref var animations = ref entity.Get<AnimationsComponent>();

            if (platesTrap.pressCount>0 && !platesTrap.wasPressed)
            {
                platesTrap.restoreTimeCurrent = 0;
                platesTrap.wasPressed = true;
                if (!animations.IsPlaying("idle-pressed"))
                {
                    animations.Play("idle-pressed");

                    world.CreateEntity(pressSfxDefinition, null, e =>
                    {
                        e.Get<PositionComponent>().value = entity.Get<PositionComponent>().value;
                    });
                }
            } else if (platesTrap.pressCount <= 0 && platesTrap.wasPressed)
            {
                platesTrap.restoreTimeCurrent += dt;

                if (platesTrap.restoreTimeCurrent > platesTrap.restoreTimeTotal)
                {
                    platesTrap.wasPressed = false;
                    if (!animations.IsPlaying("idle"))
                    {
                        animations.Play("idle");
                    
                        world.CreateEntity(unpressSfxDefinition, null, e =>
                        {
                            e.Get<PositionComponent>().value = entity.Get<PositionComponent>().value;
                        });
                    }
                }
            }
        }
    }
}