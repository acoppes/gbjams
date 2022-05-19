using GBJAM.Commons;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

namespace GBJAM9.Ecs
{
    public class InputSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var filter = world.GetFilter<UnitInputComponent>().End();
            var inputs = world.GetComponents<UnitInputComponent>();
            
            var gameboyKeyMap = GameboyInput.Instance.current;

            foreach (var entity in filter)
            {
                ref var input = ref inputs.Get(entity);
                
                input.movementDirection = gameboyKeyMap.direction;
                if (gameboyKeyMap.direction.SqrMagnitude() > 0)
                {
                    input.attackDirection = gameboyKeyMap.direction;
                }
                input.attack = gameboyKeyMap.button1Pressed;
                input.dash = gameboyKeyMap.button2Pressed;
            }
        }
    }
}
