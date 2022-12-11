using Beatemup.Ecs;
using Gemserk.Leopotam.Gameplay.Controllers;
using UnityEngine;

namespace Beatemup.Controllers
{
    public class DeathBodyController : ControllerBase
    {
        public Color color;
        
        public override void OnUpdate(float dt)
        {
            ref var model = ref world.GetComponent<UnitModelComponent>(entity);
            model.color = color;
        }
    }
}