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
                
                obstacle.body = obstacleGameObject.AddComponent<Rigidbody>();

                obstacle.body.drag = 1;
                obstacle.body.useGravity = false;
                obstacle.body.mass = obstacle.size;

                obstacle.body.constraints = RigidbodyConstraints.FreezeRotation;

                if (obstacle.isStatic)
                {
                    obstacle.body.constraints = RigidbodyConstraints.FreezeAll;
                }
                
                if (obstacle.obstacleType == UnitDefinition.ObstacleType.Circle)
                {
                    var collider = obstacleGameObject.AddComponent<SphereCollider>();
                    collider.radius = obstacle.size;
                    obstacle.collider = collider;
                } else if (obstacle.obstacleType == UnitDefinition.ObstacleType.Box)
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

                if (obstacle.body != null)
                {
                    GameObject.Destroy(obstacle.body.gameObject);
                }

                obstacle.body = null;
                obstacle.collider = null;
            }
        }
    }
}