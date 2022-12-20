using System.Collections;
using Beatemup.Definitions;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class CameraShakeSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        private GameObject worldCamera;
        
        public void Init(EcsSystems systems)
        {
            worldCamera = GameObject.FindWithTag("WorldCamera");
        }

        public void Run(EcsSystems systems)
        {
            var cameraShakeProviders = world.GetComponents<CameraShakeProvider>();
            
            foreach (var entity in world.GetFilter<CameraShakeProvider>().End())
            {
                ref var cameraShakeProvider = ref cameraShakeProviders.Get(entity);

                if (cameraShakeProvider.shake != null)
                {
                    StartCoroutine(CameraShake.Shake(cameraShakeProvider.shake, worldCamera.transform));
                    cameraShakeProvider.shake = null;
                }
            }
        }
        
        
    }
}