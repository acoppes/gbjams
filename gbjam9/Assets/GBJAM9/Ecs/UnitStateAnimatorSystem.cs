using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace GBJAM9.Ecs
{
    public class UnitStateAnimatorSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        public void Init(EcsSystems systems)
        {
            world.onEntityCreated += OnEntityCreated;
            world.onEntityDestroyed += OnEntityDestroyed;
        }
        
        private void OnEntityCreated(Gemserk.Leopotam.Ecs.World world, int entity)
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

        private void OnEntityDestroyed(Gemserk.Leopotam.Ecs.World world, int entity)
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
            }
            
        }
    }
}