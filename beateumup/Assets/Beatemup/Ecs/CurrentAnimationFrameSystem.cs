using System;
using System.Linq;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

namespace Beatemup.Ecs
{
    public class CurrentAnimationFrameSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var animations = world.GetComponents<AnimationComponent>();
            var currentFrames = world.GetComponents<CurrentAnimationFrameComponent>();
            
            foreach (var entity in world.GetFilter<AnimationComponent>().Inc<CurrentAnimationFrameComponent>().End())
            {
                var animationComponent = animations.Get(entity);
                ref var currentAnimationFrameComponent = ref currentFrames.Get(entity);
                
                currentAnimationFrameComponent.hit = false;
                
                if (animationComponent.currentAnimation != currentAnimationFrameComponent.animation || animationComponent.currentFrame != currentAnimationFrameComponent.frame)
                {
                    var asset = animationComponent.animationsAsset;
                    var animation = asset.animations[animationComponent.currentAnimation];
                    var frame = animation.frames[animationComponent.currentFrame];
                    
                    if (frame.HasEvents)
                    {
                        currentAnimationFrameComponent.hit =
                            frame.events.Contains("hit", StringComparer.OrdinalIgnoreCase);
                    }

                    currentAnimationFrameComponent.animation = animationComponent.currentAnimation;
                    currentAnimationFrameComponent.frame = animationComponent.currentFrame;
                }
            }
        }
    }
}