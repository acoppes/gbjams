using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;

namespace GBJAM10.Controllers
{
    public class BossDeathController : ControllerBase
    {
        public override void OnUpdate(float dt)
        {
            ref var control = ref world.GetComponent<UnitControlComponent>(entity);
            control.direction.x = 1;
        }
    }
}