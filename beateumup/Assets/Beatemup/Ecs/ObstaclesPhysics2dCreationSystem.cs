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
                
                obstacle.body = obstacleGameObject.AddComponent<Rigidbody2D>();
                obstacle.collider2d = obstacleGameObject.AddComponent<CircleCollider2D>();

                obstacle.body.gravityScale = 0;
                
                obstacle.collider2d.radius = obstacle.size;
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
                obstacle.collider2d = null;
            }
        }
    }
}