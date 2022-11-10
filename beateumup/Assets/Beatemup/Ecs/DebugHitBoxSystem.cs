using System;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class DebugHitBoxSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public GameObject debugHitBoxPrefab;
        public GameObject debugHurtBoxPrefab;
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<HitBoxComponent>(entity))
            {
                ref var hitBox = ref world.GetComponent<HitBoxComponent>(entity);
                
                hitBox.debugHitBox = GameObject.Instantiate(debugHitBoxPrefab);
                hitBox.debugHitBox.SetActive(true);
                hitBox.debugHitBox.transform.localScale = new Vector3(0, 0, 1);
                
                hitBox.debugHurtBox = GameObject.Instantiate(debugHurtBoxPrefab);
                hitBox.debugHurtBox.SetActive(true);
                hitBox.debugHurtBox.transform.localScale = new Vector3(0, 0, 1);
            }
        }
        
        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (world.HasComponent<HitBoxComponent>(entity))
            {
                ref var hitBox = ref world.GetComponent<HitBoxComponent>(entity);

                if (hitBox.debugHitBox != null)
                {
                    GameObject.DestroyImmediate(hitBox.debugHitBox);
                }

                if (hitBox.debugHurtBox != null)
                {
                    GameObject.DestroyImmediate(hitBox.debugHurtBox);
                }
                
                hitBox.debugHitBox = null;
                hitBox.debugHurtBox = null;
            }
        }
        
        public void Run(EcsSystems systems)
        {
            var hitBoxComponents = world.GetComponents<HitBoxComponent>();
            
            foreach (var entity in world.GetFilter<HitBoxComponent>().End())
            {
                var hitBox = hitBoxComponents.Get(entity);
                
                hitBox.debugHitBox.transform.position = hitBox.hit.position;
                hitBox.debugHitBox.transform.localScale = new Vector3(hitBox.hit.size.x, hitBox.hit.size.y, 1);
                
                hitBox.debugHurtBox.transform.position = hitBox.hurt.position;
                hitBox.debugHurtBox.transform.localScale = new Vector3(hitBox.hurt.size.x, hitBox.hurt.size.y, 1);
            }
            
        }
    }
}