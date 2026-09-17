using Game.Components;
using Game.Utilities;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using Gemserk.Utilities;

namespace GBJAM14.Controllers
{
    public class SpikesTrapController : ControllerBase, IInit, IUpdate
    {
        public void OnInit(World world, Entity entity)
        {
            var physics2DComponent = entity.Get<Physics2dComponent>();
            physics2DComponent.collisionsEventsDelegate.onCollisionEnter += OnEntityCollision;
            
            entity.Add(new SpikesTrapComponent()
            {
                active = false,
                activeCooldown = new Cooldown(2)
            });
        }

        private void OnEntityCollision(World world, Entity entity, IEntityCollisionDelegate.EntityCollision entityCollision)
        {
            ref var spikes = ref entity.Get<SpikesTrapComponent>();
            
            if (spikes.active)
            {
                return;
            }
            
            ref var animations = ref entity.Get<AnimationsComponent>();
            
            if (!animations.IsPlaying("activate"))
            {
                animations.Play("activate", 1);
            }
            
            if (entityCollision.entity)
            {
                if (entityCollision.entity.Has<HealthComponent>())
                {
                    entityCollision.entity.Get<HealthComponent>().damages.Add(new HealthChangeData()
                    {
                        position = entityCollision.collider2D.transform.position,
                        player = 0,
                        knockback = true,
                        source = entity,
                        value = 1,
                        vfxDefinition = null
                    });
                }
            }

            spikes.active = true;
            spikes.activeCooldown.Reset();
        }

        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var spikes = ref entity.Get<SpikesTrapComponent>();
        
            ref var animations = ref entity.Get<AnimationsComponent>();
            
            if (animations.IsPlaying("return") && animations.isCompleted)
            {
                animations.Play("idle");
                spikes.active = false;
                return;
            }
            
            if (spikes.active)
            {
                spikes.activeCooldown.Increase(dt);
                if (spikes.activeCooldown.IsReady)
                {
                    if (!animations.IsPlaying("return"))
                    {
                        animations.Play("return", 1);
                    }
                }
            }
        }
    }
}