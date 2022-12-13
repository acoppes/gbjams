using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class ModelShakeSystem : BaseSystem, IEcsRunSystem
    {
        public float maxIntensityMultiplier = 0.5f;

        public AnimationCurve intensityCurve = 
            AnimationCurve.Linear(0, 1, 0, 0);
        
        public void Run(EcsSystems systems)
        {
            var modelComponents = world.GetComponents<UnitModelComponent>();
            var modelShakeComponents = world.GetComponents<ModelShakeComponent>();

            var dt = Time.deltaTime;
            
            foreach (var entity in world.GetFilter<UnitModelComponent>().Inc<ModelShakeComponent>().End())
            {
                ref var modelComponent = ref modelComponents.Get(entity);
                ref var modelShakeComponent = ref modelShakeComponents.Get(entity);

                modelShakeComponent.time += dt;

                if (modelShakeComponent.time < modelShakeComponent.duration)
                {
                    var intensity = intensityCurve.Evaluate(modelShakeComponent.time / modelShakeComponent.duration);

                    var direction = UnityEngine.Random.Range(-1.0f, 1.0f) < 0 ? -1f : 1f;
                    var randomPosition = Vector2.right * maxIntensityMultiplier * intensity * direction;
                    
                    modelComponent.instance.model.transform.localPosition += 
                        new Vector3(randomPosition.x, 0, 0);
                } 
            }
        }
    }
}