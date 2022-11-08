using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

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

                var asset = animationComponent.animationsAsset;
                var animation = asset.animations[animationComponent.currentAnimation];
                var frame = animation.frames[animationComponent.currentFrame];

                currentAnimationFrameComponent.hit = frame.hitEvent;
            }
        }
    }
}