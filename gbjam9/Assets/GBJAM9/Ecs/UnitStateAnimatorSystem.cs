using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace GBJAM9.Ecs
{
    public class UnitStateAnimatorSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public void OnEntityCreated(Gemserk.Leopotam.Ecs.World world, int entity)
        {
            var models = world.GetComponents<UnitModelComponent>();
            var animators = world.GetComponents<AnimatorComponent>();
            
            if (models.Has(entity) && animators.Has(entity))
            {
                var model = models.Get(entity);
                ref var animator = ref animators.Get(entity);
                animator.animator = model.instance.GetComponent<Animator>();
            }
        }

        public void OnEntityDestroyed(Gemserk.Leopotam.Ecs.World world, int entity)
        {
            var animators = world.GetComponents<AnimatorComponent>();
            
            if (animators.Has(entity))
            {
                ref var animator = ref animators.Get(entity);
                animator.animator = null;
            }
        }

        public void Run(EcsSystems systems)
        {
            var animators = world.GetComponents<AnimatorComponent>();
            var states = world.GetComponents<UnitStateComponent>();

            foreach (var entity in world
                         .GetFilter<AnimatorComponent>()
                         .Inc<UnitStateComponent>()
                         .End())
            { 
                var animatorComponent = animators.Get(entity);
                var unitStateComponent = states.Get(entity);
                
                animatorComponent.animator
                    .SetBool("walking", unitStateComponent.walking);
                animatorComponent.animator
                    .SetBool("dashing", unitStateComponent.dashing);
                animatorComponent.animator
                    .SetBool("sword_attack", unitStateComponent.attacking1);
                animatorComponent.animator
                    .SetBool("charge_attack1", unitStateComponent.chargeAttack1);

                if (unitStateComponent.stateTriggers.hit)
                {
                    animatorComponent.animator.SetTrigger("hitted");
                }

                if (unitStateComponent.healthState == HealthComponent.State.Death)
                {
                    animatorComponent.animator.SetTrigger("dead");
                }
            }
            
        }
    }
}