using Beatemup.Development;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class DebugHitBoxSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public bool debugHitBoxesEnabled;
        
        public GameObject hitBoxDebugPrefab;
        
        public DebugHitBox CreateDebugHitBox(int type)
        {
            var debugHitBoxInstance = GameObject.Instantiate(hitBoxDebugPrefab);
            debugHitBoxInstance.SetActive(true);

            var debugHitBox = debugHitBoxInstance.GetComponent<DebugHitBox>();
            debugHitBox.debugHitBoxSystem = this;
            debugHitBox.type = type;
            
            return debugHitBox;
        }

        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<HitBoxComponent>(entity))
            {
                ref var hitBox = ref world.GetComponent<HitBoxComponent>(entity);

                hitBox.debugHitBox = CreateDebugHitBox(0);
                hitBox.debugHurtBox = CreateDebugHitBox(1);
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
                
                hitBox.debugHitBox.UpdateHitBox(hitBox.hit);
                hitBox.debugHurtBox.UpdateHitBox(hitBox.hurt);
            }
        }
    }
}