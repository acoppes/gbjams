using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class JumpSystem : BaseSystem, IEcsRunSystem
    {
        public AnimationCurve upCurve;
        public AnimationCurve fallCurve;
        
        public void Run(EcsSystems systems)
        {
            var jumpComponents = world.GetComponents<JumpComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            
            foreach (var entity in world.GetFilter<JumpComponent>().Inc<PositionComponent>().End())
            {
                ref var jump = ref jumpComponents.Get(entity);
                ref var position = ref positionComponents.Get(entity);

                if (!jump.isActive)
                {
                    jump.upTime = 0;
                }
                else
                {
                    jump.fallTime = 0;
                }

                if (!jump.disabled)
                {
                    if (jump.isActive)
                    {
                        var currentSpeed = upCurve.Evaluate(jump.upTime) * jump.upSpeed;
                        position.value.z += currentSpeed * Time.deltaTime;

                        jump.upTime += Time.deltaTime;
                    }
                    else if (position.value.z > 0)
                    {
                        var currentSpeed = fallCurve.Evaluate(jump.fallTime) * jump.fallSpeed;
                        position.value.z -= currentSpeed * Time.deltaTime;
                        
                        jump.fallTime += Time.deltaTime;
                    }
                }

                if (position.value.z < 0)
                {
                    position.value.z = 0;
                }
                
                jump.isOverGround = position.value.z <= Mathf.Epsilon;
            }
        }
    }
}