using Game.Components;
using Game.Controllers;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;

namespace GBJAM11.Controllers
{
    public class DeathController : ControllerBase, IUpdate, IActiveController, IInit, IDestroyed
    {
        public bool destroyOnAnimationCompleted; 
        
        public void OnInit(World world, Entity entity)
        {
            ref var health = ref entity.Get<HealthComponent>();
            health.onDeathEvent+= OnDeath;
        }

        public void OnDestroyed(World world, Entity entity)
        {
            ref var health = ref entity.Get<HealthComponent>();
            health.onDeathEvent -= OnDeath;
        }
        
        private void OnDeath(World world, Entity entity)
        {
            EnterDeath(world, entity);
        }

        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();

            if (states.TryGetState("Death", out var deathState))
            {
                if (animations.IsPlaying("Death") && animations.isCompleted)
                {
                    if (destroyOnAnimationCompleted)
                    {
                        entity.Get<DestroyableComponent>().destroy = true;
                    }
                }

                return;
            }
        }

        private void EnterDeath(World world, Entity entity)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            ref var activeController = ref entity.Get<ActiveControllerComponent>();
            ref var movement = ref entity.Get<MovementComponent>();
            
            activeController.TakeControl(entity, this);
            movement.speed = 0;

            animations.Play("Death", 0);
            states.EnterState("Death");
        }
        
        public bool CanBeInterrupted(Entity entity, IActiveController activeController)
        {
            return false;
        }

        public void OnInterrupt(Entity entity, IActiveController activeController)
        {
            throw new System.NotImplementedException();
        }



    }
}