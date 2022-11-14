using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class JumpSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var jumpComponents = world.GetComponents<JumpComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            
            foreach (var entity in world.GetFilter<JumpComponent>().Inc<PositionComponent>().End())
            {
                ref var jump = ref jumpComponents.Get(entity);
                ref var position = ref positionComponents.Get(entity);
                
                if (!jump.disabled)
                {
                    if (jump.isActive)
                    {
                        position.value.z += jump.speed * Time.deltaTime;
                    }
                    else if (position.value.z > 0)
                    {
                        position.value.z -= jump.speed * Time.deltaTime;
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