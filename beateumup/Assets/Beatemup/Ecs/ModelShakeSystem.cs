using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class ModelShakeSystem : BaseSystem, IEcsRunSystem
    {
        public float maxIntensityMultiplier = 0.5f;

        public float frameUpdateTime = 2.0f / 15.0f;
        private float frameUpdateCurrent = 0;

        public AnimationCurve intensityCurve = 
            AnimationCurve.Linear(0, 1, 0, 0);
        
        public void Run(EcsSystems systems)
        {
            var modelComponents = world.GetComponents<UnitModelComponent>();
            var modelShakeComponents = world.GetComponents<ModelShakeComponent>();

            var dt = Time.deltaTime;

            frameUpdateCurrent += dt;
            
            foreach (var entity in world.GetFilter<UnitModelComponent>().Inc<ModelShakeComponent>().End())
            {
                ref var modelComponent = ref modelComponents.Get(entity);
                ref var modelShakeComponent = ref modelShakeComponents.Get(entity);

                modelShakeComponent.time += dt;

                if (modelShakeComponent.time < modelShakeComponent.duration)
                {
                    if (frameUpdateCurrent >= frameUpdateTime)
                    {
                        var intensity =
                            intensityCurve.Evaluate(modelShakeComponent.time / modelShakeComponent.duration);

                        // flip
                        var direction = modelShakeComponent.currentOffset.x < 0 ? 1.0f : -1.0f;
                        var randomPosition = Vector2.right * maxIntensityMultiplier * intensity * direction;

                        modelShakeComponent.currentOffset = new Vector3(randomPosition.x, 0, 0);
                    }
                }
                else
                {
                    modelShakeComponent.currentOffset = Vector3.zero;
                }
                
                modelComponent.instance.model.transform.localPosition += 
                    modelShakeComponent.currentOffset;
            }

            if (frameUpdateCurrent >= frameUpdateTime)
            {
                frameUpdateCurrent = 0;
            }
        }
    }
}