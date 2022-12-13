using Beatemup.Definitions;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class PhysicsCreationSystem : BaseSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public PhysicMaterial defaultMaterial;
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<PhysicsComponent>(entity))
            {
                ref var physicsComponent = ref world.GetComponent<PhysicsComponent>(entity);
                
                var physicsGameObject = new GameObject("~PhysicsObject");
                physicsGameObject.layer = LayerMask.NameToLayer("Obstacle");

                if (physicsComponent.isStatic)
                {
                    physicsComponent.body = null;
                    physicsGameObject.isStatic = true;
                 
                    // obstacle.body.constraints = RigidbodyConstraints.FreezeAll;
                }
                else
                {
                    physicsComponent.body = physicsGameObject.AddComponent<Rigidbody>();

                    // physicsComponent.body.drag = 0;
                    physicsComponent.body.angularDrag = 10;
                    physicsComponent.body.useGravity = false;
                    physicsComponent.body.mass = physicsComponent.size;

                    physicsComponent.body.constraints = RigidbodyConstraints.FreezeRotation;
                }
                
                if (physicsComponent.shapeType == PhysicsComponent.ShapeType.Circle)
                {
                    var collider = physicsGameObject.AddComponent<SphereCollider>();
                    collider.radius = physicsComponent.size;
                    collider.center = new Vector3(0, collider.radius, 0);
                    collider.sharedMaterial = defaultMaterial;
                    
                    physicsComponent.collider = collider;
                } else if (physicsComponent.shapeType == PhysicsComponent.ShapeType.Box)
                {
                    var collider = physicsGameObject.AddComponent<BoxCollider>();
                    collider.size = new Vector4(physicsComponent.size, physicsComponent.size, physicsComponent.size);
                    collider.sharedMaterial = defaultMaterial;
                    physicsComponent.collider = collider;
                }
            }
        }
        
        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (world.HasComponent<PhysicsComponent>(entity))
            {
                ref var physicsComponent = ref world.GetComponent<PhysicsComponent>(entity);

                if (physicsComponent.collider != null)
                {
                    GameObject.Destroy(physicsComponent.collider.gameObject);
                }

                physicsComponent.body = null;
                physicsComponent.collider = null;
            }
        }
    }
}