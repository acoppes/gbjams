using Beatemup.Ecs;
using Gemserk.Leopotam.Gameplay.Controllers;
using UnityEngine;

namespace Beatemup.Controllers
{
    public class DeathBodyController : ControllerBase, IUpdate
    {
        public Color color;
        
        public void OnUpdate(float dt)
        {
            ref var model = ref world.GetComponent<UnitModelComponent>(entity);
            model.color = color;
        }
    }
}