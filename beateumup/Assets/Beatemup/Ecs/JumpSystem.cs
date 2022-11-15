using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class JumpSystem : BaseSystem, IEcsRunSystem
    {
        public AnimationCurve upCurve;
        
        public void Run(EcsSystems systems)
        {
            var jumpComponents = world.GetComponents<JumpComponent>();
            var verticalComponents = world.GetComponents<VerticalMovementComponent>();
            
            foreach (var entity in world.GetFilter<JumpComponent>().Inc<VerticalMovementComponent>().End())
            {
                ref var jump = ref jumpComponents.Get(entity);
                ref var verticalMovement = ref verticalComponents.Get(entity);

                if (!jump.isActive)
                {
                    jump.upTime = 0;
                }
                
                if (jump.isActive)
                {
                    // var currentSpeed = upCurve.Evaluate(jump.upTime) * jump.upSpeed;

                    verticalMovement.speed = upCurve.Evaluate(jump.upTime) * jump.upSpeed;
                        
                    // verticalMovement.value.z += currentSpeed * Time.deltaTime;

                    jump.upTime += Time.deltaTime;
                }
            }
        }
    }
}