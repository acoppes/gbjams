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
                
                obstacle.body = obstacleGameObject.AddComponent<Rigidbody2D>();

                obstacle.body.gravityScale = 0;
                obstacle.body.mass = obstacle.size;

                obstacle.body.bodyType = obstacle.isStatic ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic; 
                
                if (obstacle.obstacleType == UnitDefinition.ObstacleType.Circle)
                {
                    var collider2d = obstacleGameObject.AddComponent<CircleCollider2D>();
                    collider2d.radius = obstacle.size;
                    obstacle.collider2d = collider2d;
                } else if (obstacle.obstacleType == UnitDefinition.ObstacleType.Box)
                {
                    var collider2d = obstacleGameObject.AddComponent<BoxCollider2D>();
                    collider2d.size = new Vector2(obstacle.size, obstacle.size);
                    obstacle.collider2d = collider2d;
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
                obstacle.collider2d = null;
            }
        }
    }
}