using Game.Components;
using GBJAM11.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace GBJAM11.Systems
{
    public class AutoUnitAnimationsSystem : BaseSystem, IEcsRunSystem
    {
        private const string WALK_ANIMATION = "Walk";
        private const string IDLE_ANIMATION = "Idle";

        readonly EcsFilterInject<Inc<MovementComponent, AnimationComponent, AutoAnimationComponent>, Exc<DisabledComponent>> animationFilter = default;
        readonly EcsFilterInject<Inc<MovementComponent, LookingDirection>, Exc<DisabledComponent>> lookingDirectionFilter = default;
        readonly EcsFilterInject<Inc<ActiveControllerComponent, AnimationComponent, MovementComponent, AutoAnimationComponent>, Exc<DisabledComponent>> abilitiesFilter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var e in animationFilter.Value)
            {
                ref var movement = ref animationFilter.Pools.Inc1.Get(e);
                ref var animations = ref animationFilter.Pools.Inc2.Get(e);

                if (animationFilter.Pools.Inc3.Get(e).disabled)
                    continue;

                if (movement.isMoving)
                {
                    // var walkAnimation = 
                    //     animation.animationsAsset.GetDirectionalAnimation(WALK_ANIMATION, 
                    //         movement.movingDirection);

                    if (!animations.IsPlaying(WALK_ANIMATION))
                    {
                        animations.Play(WALK_ANIMATION);
                    }
                }
            }
            
            foreach (var e in lookingDirectionFilter.Value)
            {
                ref var movement = ref lookingDirectionFilter.Pools.Inc1.Get(e);
                ref var lookingDirection = ref lookingDirectionFilter.Pools.Inc2.Get(e);

                if (movement.isMoving)
                {
                    lookingDirection.value = movement.movingDirection.normalized;
                }
            }
            
            foreach (var e in abilitiesFilter.Value)
            {
                var activeController = abilitiesFilter.Pools.Inc1.Get(e);
                ref var animations = ref abilitiesFilter.Pools.Inc2.Get(e);
                var movement = abilitiesFilter.Pools.Inc3.Get(e);
                
                if (abilitiesFilter.Pools.Inc4.Get(e).disabled)
                    continue;
                
                // var executingAbility = !activeController.IsControlled();
                
                // foreach (var ability in abilities.abilities)
                // {
                //     if (ability.isExecuting)
                //     {
                //         executingAbility = true;
                //         break;
                //     }    
                // }

                if (!activeController.IsControlled())
                {
                    if (!movement.isMoving && !animations.IsPlaying(IDLE_ANIMATION))
                    {
                        animations.Play(IDLE_ANIMATION);
                    }
                }
            }
        }
    }
}