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

        /*public void Init(EcsSystems systems)
        {
            instance = this;
        }

        public void Destroy(EcsSystems systems)
        {
            instance = null;
        }*/
        
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
            // var positionComponents = world.GetComponents<PositionComponent>();
            
            // foreach (var entity in world.GetFilter<HitBoxComponent>().End())
            // {
            //     var hitBox = hitBoxComponents.Get(entity);
            //     hitBox.debugHitBox.gameObject.SetActive(debugHitBoxesEnabled);
            //     hitBox.debugHurtBox.gameObject.SetActive(debugHitBoxesEnabled);
            //     hitBox.debugDepthBox.gameObject.SetActive(debugHitBoxesEnabled);
            // }
            
            foreach (var entity in world.GetFilter<HitBoxComponent>().End())
            {
                var hitBox = hitBoxComponents.Get(entity);
                
                hitBox.debugHitBox.UpdateHitBox(hitBox.hit);
                hitBox.debugHurtBox.UpdateHitBox(hitBox.hurt);
            }
            
            // foreach (var entity in world.GetFilter<HitBoxComponent>().Inc<PositionComponent>().End())
            // {
            //     var hitBox = hitBoxComponents.Get(entity);
            //     var position = positionComponents.Get(entity);
            //     
            //     hitBox.debugDepthBox.UpdateHitBox(new HitBox()
            //     {
            //         
            //     });
            //     hitBox.debugDepthBox.transform.localScale = new Vector3(hitBox.hurt.size.x, hitBox.hurt.depth * 2.0f, 1);
            // }
            
        }


    }
}