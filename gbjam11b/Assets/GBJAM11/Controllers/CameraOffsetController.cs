using GBJAM11.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using MyBox;

namespace GBJAM11.Controllers
{
    public class CameraOffsetController : ControllerBase, IUpdate
    {
        public float offsetDistance = 1.5f;
        
        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var cameraOffset = ref entity.Get<CameraOffsetComponent>();
            var lookingDirection = entity.Get<LookingDirection>().value.ToVector2();
            
            if (lookingDirection.sqrMagnitude > 0)
            {
                cameraOffset.offset = lookingDirection.normalized * offsetDistance;
            }
        }
    }
}