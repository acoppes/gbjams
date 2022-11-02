using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class UnitStateAnimatorSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public void OnEntityCreated(Gemserk.Leopotam.Ecs.World world, Entity entity)
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

        public void OnEntityDestroyed(Gemserk.Leopotam.Ecs.World world, Entity entity)
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
            var states = world.GetComponents<ModelStateComponent>();

            foreach (var entity in world
                         .GetFilter<AnimatorComponent>()
                         .Inc<ModelStateComponent>()
                         .End())
            { 
                var animatorComponent = animators.Get(entity);
                var unitStateComponent = states.Get(entity);
                
                animatorComponent.animator
                    .SetBool(nameof(unitStateComponent.walking), unitStateComponent.walking);
                
                animatorComponent.animator
                    .SetBool(nameof(unitStateComponent.up), unitStateComponent.up);
                
                animatorComponent.animator
                    .SetBool(nameof(unitStateComponent.dashing), unitStateComponent.dashing);
                
                animatorComponent.animator
                    .SetBool(nameof(unitStateComponent.attack), unitStateComponent.attack);
                
                animatorComponent.animator
                    .SetBool(nameof(unitStateComponent.attackMoving), unitStateComponent.attackMoving);
                
                animatorComponent.animator
                    .SetBool(nameof(unitStateComponent.sprinting), unitStateComponent.sprinting);

                if (unitStateComponent.stateTriggers.hit)
                {
                    animatorComponent.animator.SetTrigger("hitted");
                }


            }
            
        }
    }
}