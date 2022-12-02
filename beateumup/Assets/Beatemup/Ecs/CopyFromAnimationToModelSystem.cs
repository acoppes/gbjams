using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

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

                if (animationComponent.currentAnimation == AnimationComponent.NoAnimation)
                {
                    continue;
                }
                
                var animation = animationComponent.animationsAsset.animations[animationComponent.currentAnimation];
                var frame = animation.frames[animationComponent.currentFrame];
                
                if (modelComponent.instance.model != null)
                {
                    modelComponent.instance.model.sprite = frame.sprite;
                }
                
                if (modelComponent.instance.effect != null)
                {
                    modelComponent.instance.effect.sprite = frame.fxSprite;
                }
            }
        }
    }
}