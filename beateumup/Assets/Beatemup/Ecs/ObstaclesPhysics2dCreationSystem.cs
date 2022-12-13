using Beatemup.Definitions;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class ObstaclesPhysics2dCreationSystem : BaseSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<ObstacleComponent>(entity))
            {
                ref var obstacle = ref world.GetComponent<ObstacleComponent>(entity);
                
                var obstacleGameObject = new GameObject("~Obstacle");
                obstacleGameObject.layer = LayerMask.NameToLayer("Obstacle");

              

                if (obstacle.isStatic)
                {
                    obstacle.body = null;
                    obstacleGameObject.isStatic = true;
                 
                    // obstacle.body.constraints = RigidbodyConstraints.FreezeAll;
                }
                else
                {
                    obstacle.body = obstacleGameObject.AddComponent<Rigidbody>();

                    obstacle.body.drag = 10;
                    obstacle.body.angularDrag = 10;
                    obstacle.body.useGravity = false;
                    obstacle.body.mass = obstacle.size;

                    obstacle.body.constraints = RigidbodyConstraints.FreezeRotation;
                }
                
                if (obstacle.obstacleType == ObstacleComponent.ObstacleType.Circle)
                {
                    var collider = obstacleGameObject.AddComponent<SphereCollider>();
                    collider.radius = obstacle.size;
                    obstacle.collider = collider;
                } else if (obstacle.obstacleType == ObstacleComponent.ObstacleType.Box)
                {
                    var collider = obstacleGameObject.AddComponent<BoxCollider>();
                    collider.size = new Vector4(obstacle.size, obstacle.size, obstacle.size);
                    obstacle.collider = collider;
                }
            }
        }
        
        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (world.HasComponent<ObstacleComponent>(entity))
            {
                ref var obstacle = ref world.GetComponent<ObstacleComponent>(entity);

                if (obstacle.collider != null)
                {
                    GameObject.Destroy(obstacle.collider.gameObject);
                }

                obstacle.body = null;
                obstacle.collider = null;
            }
        }
    }
}