using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class UnitStateAnimatorSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        private readonly int _walkingParameterHash = Animator.StringToHash("walking");
        private readonly int _upParameterHash = Animator.StringToHash("up");
        
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
                    .SetBool(_walkingParameterHash, unitStateComponent.walking);
                
                animatorComponent.animator
                    .SetBool(_upParameterHash, unitStateComponent.up);

                if (unitStateComponent.stateTriggers.hit)
                {
                    animatorComponent.animator.SetTrigger("hitted");
                }


            }
            
        }
    }
}