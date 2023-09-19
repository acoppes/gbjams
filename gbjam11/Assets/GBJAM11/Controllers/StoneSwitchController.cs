using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using UnityEngine;

namespace GBJAM11.Controllers
{
    public class StoneSwitchController : ControllerBase, IUpdate
    {
        public void OnUpdate(World world, Entity entity, float dt)
        {
            var physics = entity.Get<PhysicsComponent>();

            if (physics.contactsCount > 0)
            {
                Debug.Log("IN CONTACT");
            }
        }
    }
}