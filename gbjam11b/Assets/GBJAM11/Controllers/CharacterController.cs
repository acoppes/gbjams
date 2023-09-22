using Game.Components;
using Game.Controllers;
using GBJAM11.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using MyBox;

namespace GBJAM11.Controllers
{
    public class CharacterController : ControllerBase, IUpdate, IActiveController
    {
        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var input = ref entity.Get<InputComponent>();
            ref var bufferedInput = ref entity.Get<BufferedInputComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            ref var weapons = ref entity.Get<WeaponsComponent>();
            
            // if attacking 
            // fire attack

            ref var movement = ref entity.Get<MovementComponent>();
            movement.movingDirection = input.direction().vector2;
        }

        public bool CanBeInterrupted(Entity entity, IActiveController activeController)
        {
            throw new System.NotImplementedException();
        }

        public void OnInterrupt(Entity entity, IActiveController activeController)
        {
            throw new System.NotImplementedException();
        }
    }
}