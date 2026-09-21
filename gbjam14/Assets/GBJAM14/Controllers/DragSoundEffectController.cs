using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using UnityEngine;

namespace GBJAM14.Controllers
{
    public class DragSoundEffectController : ControllerBase, IUpdate, IInit
    {
        [EntityDefinition]
        public Object dragSfx;

        private Entity dragSfxInstance;
        private Vector3 lastPosition;

        private float moveCheckTime = 0.1f;
        private float moveCheckCurrent;
        
        public void OnInit(World world, Entity entity)
        {
            lastPosition = entity.Get<PositionComponent>().value;
        }

        public void OnUpdate(World world, Entity entity, float dt)
        {
            var position = entity.Get<PositionComponent>();

            // var moving = physics2DComponent.body.linearVelocity.sqrMagnitude > 0.01f;

            var moved = (lastPosition - position.value).sqrMagnitude > 0.01f;
            if (moved)
            {
                lastPosition = position.value;
                moveCheckCurrent = 0;
            }

            if (!moved)
            {
                moveCheckCurrent += dt;
                if (moveCheckCurrent < moveCheckTime)
                {
                    moved = true;
                }
            }
            
            if (moved && !dragSfxInstance)
            {
                dragSfxInstance = world.CreateEntity(dragSfx, null);
            } else if (!moved && dragSfxInstance)
            {
                dragSfxInstance.Get<DestroyableComponent>().destroy = true;
                dragSfxInstance = Entity.NullEntity;
            }

            if (dragSfxInstance)
            {
                dragSfxInstance.Get<PositionComponent>().value = entity.Get<PositionComponent>().value;
            }
        }


    }
}