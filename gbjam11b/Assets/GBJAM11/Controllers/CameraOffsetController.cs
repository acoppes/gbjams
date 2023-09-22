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
            ref var cameraOffset = ref entity.Get<CameraOffsetComponent>();
            var offset = Vector2.zero;
            
            offset.x = entity.Get<LookingDirection>().value.x >= 0 ? cameraOffsetMaxValue.x : -cameraOffsetMaxValue.x;
            offset.y = entity.Get<LookingDirection>().value.y >= 0 ? cameraOffsetMaxValue.y : -cameraOffsetMaxValue.y;

            cameraOffset.offset = offset;
        }
    }
}