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
            }
            
            if (world.HasComponent<HurtBoxComponent>(entity))
            {
                ref var hurtBox = ref world.GetComponent<HurtBoxComponent>(entity);
                
                hurtBox.debug = GameObject.Instantiate(debugHurtBoxPrefab);
                hurtBox.debug.SetActive(true);

                hurtBox.debug.transform.localScale = new Vector3(0, 0, 1);
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
                
                hitBox.debugHitBox = null;
            }
            
            if (world.HasComponent<HurtBoxComponent>(entity))
            {
                ref var hurtBox = ref world.GetComponent<HurtBoxComponent>(entity);

                if (hurtBox.debug != null)
                {
                    GameObject.DestroyImmediate(hurtBox.debug);
                }

                hurtBox.debug = null;
            }
        }
        
        public void Run(EcsSystems systems)
        {
            var hitBoxComponents = world.GetComponents<HitBoxComponent>();
            
            foreach (var entity in world.GetFilter<HitBoxComponent>().End())
            {
                var hitBox = hitBoxComponents.Get(entity);
                
                var debugObject = hitBox.debugHitBox;
                
                debugObject.transform.position = hitBox.hit.position;
                debugObject.transform.localScale = new Vector3(hitBox.hit.size.x, hitBox.hit.size.y, 1);
            }
            
            var hurtBoxComponents = world.GetComponents<HurtBoxComponent>();
            
            foreach (var entity in world.GetFilter<HurtBoxComponent>().End())
            {
                var hurtBox = hurtBoxComponents.Get(entity);
                
                var debugObject = hurtBox.debug;
                
                debugObject.transform.position = hurtBox.position;
                debugObject.transform.localScale = new Vector3(hurtBox.size.x, hurtBox.size.y, 1);
            }
        }
    }
}