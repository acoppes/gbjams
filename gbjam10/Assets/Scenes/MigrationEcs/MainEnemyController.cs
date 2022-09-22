using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;

public class MainEnemyController : ControllerBase
{
    public override void OnUpdate(float dt)
    {
        ref var states = ref world.GetComponent<StatesComponent>(entity);
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);

        control.direction.x = 1;
    }
}