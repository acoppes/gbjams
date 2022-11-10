using System;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class DebugHitBoxSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler
    {
        public GameObject debugHitBoxPrefab;
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<HitBoxComponent>(entity))
            {
                ref var hitBox = ref world.GetComponent<HitBoxComponent>(entity);
                
                hitBox.debug = GameObject.Instantiate(debugHitBoxPrefab);
                hitBox.debug.SetActive(true);

                hitBox.debug.transform.localScale = new Vector3(0, 0, 1);
            }
        }
        
        public void Run(EcsSystems systems)
        {
            var hitBoxComponents = world.GetComponents<HitBoxComponent>();
            
            foreach (var entity in world.GetFilter<HitBoxComponent>().End())
            {
                var hitBox = hitBoxComponents.Get(entity);
                
                var debugObject = hitBox.debug;
                
                debugObject.transform.position = hitBox.position;
                debugObject.transform.localScale = new Vector3(hitBox.size.x, hitBox.size.y, 1);
            }
        }


    }
}