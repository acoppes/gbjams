using System.Diagnostics.CodeAnalysis;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Beatemup.Ecs
{
    [SuppressMessage("ReSharper", "LocalVariableHidesMember")]
    public class PhysicsCreationSystem : BaseSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public PhysicMaterial defaultMaterial;

        private Collider CreateCollider(int layer, PhysicsComponent physicsComponent)
        {
            if (physicsComponent.shapeType == PhysicsComponent.ShapeType.Circle)
            {
                var colliderObject = new GameObject("DynamicCollider");
                colliderObject.layer = layer;
                
                // colliderObject.transform.parent = physicsComponent.transform;

                var collider = colliderObject.AddComponent<SphereCollider>();
                collider.radius = physicsComponent.size;
                collider.center = new Vector3(0, collider.radius, 0);
                collider.sharedMaterial = defaultMaterial;

                return collider;
            } else if (physicsComponent.shapeType == PhysicsComponent.ShapeType.Box)
            {
                var colliderObject = new GameObject("DynamicCollider");
                colliderObject.layer = layer;
                // colliderObject.transform.parent = physicsGameObject.transform;
                    
                var collider = colliderObject.AddComponent<BoxCollider>();
                collider.size = new Vector4(physicsComponent.size, physicsComponent.size, physicsComponent.size);
                collider.sharedMaterial = defaultMaterial;
                
                return collider;
            }

            return null;
        }
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<PhysicsComponent>(entity))
            {
                ref var physicsComponent = ref world.GetComponent<PhysicsComponent>(entity);
                
                physicsComponent.gameObject = new GameObject("~PhysicsObject");
                var layer = physicsComponent.isStatic ? LayerMask.NameToLayer("StaticObstacle") : 
                    LayerMask.NameToLayer("DynamicObstacle");
                
                if (physicsComponent.isStatic)
                {
                    physicsComponent.body = null;
                    physicsComponent.gameObject.isStatic = true;
                 
                    // obstacle.body.constraints = RigidbodyConstraints.FreezeAll;
                }
                else
                {
                    physicsComponent.body = physicsComponent.gameObject.AddComponent<Rigidbody>();

                    // physicsComponent.body.drag = 0;
                    physicsComponent.body.angularDrag = 10;
                    physicsComponent.body.useGravity = false;
                    physicsComponent.body.mass = physicsComponent.size;

                    physicsComponent.body.constraints = RigidbodyConstraints.FreezeRotation;
                }

                physicsComponent.obstacleCollider = CreateCollider(layer, physicsComponent);
                physicsComponent.obstacleCollider.transform.parent = physicsComponent.gameObject.transform;
                
                if (!physicsComponent.isStatic)
                {
                    physicsComponent.collideWithStaticCollider =
                        CreateCollider(LayerMask.NameToLayer("CollideWithStaticObstacles"), physicsComponent);
                    physicsComponent.collideWithStaticCollider.transform.parent = physicsComponent.gameObject.transform;
                }
            }
        }
        
        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (world.HasComponent<PhysicsComponent>(entity))
            {
                ref var physicsComponent = ref world.GetComponent<PhysicsComponent>(entity);

                if (physicsComponent.gameObject != null)
                {
                    GameObject.Destroy(physicsComponent.gameObject);
                }

                physicsComponent.gameObject = null;
                physicsComponent.body = null;
                physicsComponent.obstacleCollider = null;
                physicsComponent.collideWithStaticCollider = null;
            }
        }
    }
}