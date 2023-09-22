using Game.Components;
using GBJAM11.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using UnityEngine;

namespace GBJAM11.Controllers
{
    public class CameraOffsetController : ControllerBase, IUpdate
    {
        public Vector2 cameraOffsetMaxValue = new Vector2(1.5f, 1.5f);
        
        public void OnUpdate(World world, Entity entity, float dt)
        {
            var gravity = entity.Get<GravityComponent>();

            ref var cameraOffset = ref entity.Get<CameraOffsetComponent>();
            var offset = Vector2.zero;

            offset.x = entity.Get<LookingDirection>().value.x >= 0 ? cameraOffsetMaxValue.x : -cameraOffsetMaxValue.x;

            if (gravity.inContactWithGround)
            {
                offset.y = cameraOffsetMaxValue.y;
            } else if (entity.Get<StatesComponent>().HasState("OnRoof"))
            {
                offset.y = -cameraOffsetMaxValue.y;
            }
            
            // TODO: if on wall or on roof, then set the offset differently.

            cameraOffset.offset = offset;
        }
    }
}