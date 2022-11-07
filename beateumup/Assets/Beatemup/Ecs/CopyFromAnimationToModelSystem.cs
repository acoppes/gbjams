using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class CopyFromAnimationToModelSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var animations = world.GetComponents<AnimationComponent>();
            var models = world.GetComponents<UnitModelComponent>();
            
            foreach (var entity in world.GetFilter<AnimationComponent>().Inc<UnitModelComponent>().End())
            {
                var animationComponent = animations.Get(entity);
                ref var modelComponent = ref models.Get(entity);

                var animation = animationComponent.animationsAsset.animations[animationComponent.currentAnimation];
                var frame = animation.frames[animationComponent.currentFrame];

                var model = modelComponent.instance.transform.Find("Model");
                var effectModel = modelComponent.instance.transform.Find("Effect");

                if (model != null)
                {
                    model.GetComponent<SpriteRenderer>().sprite = frame.sprite;
                }
                
                if (effectModel != null)
                {
                    effectModel.GetComponent<SpriteRenderer>().sprite = frame.fxSprite;
                }
            }
        }
    }
}