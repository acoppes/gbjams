using Beatemup.Definitions;
using Beatemup.Ecs;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;
using UnityEngine;

namespace Beatemup.Controllers
{
    public class ScreenBorderController : ControllerBase, IInit
    {
        public CameraShakeAsset collisionCameraShakeAsset;

        public float minImpactSpeedToShake = 50f;
        
        public void OnInit()
        {
            var physicsComponent = Get<PhysicsComponent>();
            physicsComponent.collisionsEventsDelegate.onCollisionEnter += OnEntityCollisionEnter;
        }

        private void OnEntityCollisionEnter(EntityCollisionDelegate.EntityCollision collision)
        {
            var physicsComponent = world.GetComponent<PhysicsComponent>(collision.entity);
            
            if (physicsComponent.velocity.sqrMagnitude > minImpactSpeedToShake)
            {
                Get<CameraShakeProvider>().AddShake(collisionCameraShakeAsset);
            }
        }
    }
}