using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class CurrentAnimationAttackSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var animations = world.GetComponents<AnimationComponent>();
            var currentFrames = world.GetComponents<CurrentAnimationAttackComponent>();
            var hitBoxes = world.GetComponents<HitBoxComponent>();
            
            foreach (var entity in world.GetFilter<AnimationComponent>()
                         .Inc<CurrentAnimationAttackComponent>().Inc<HitBoxComponent>().End())
            {
                var animationComponent = animations.Get(entity);
                var hitBox = hitBoxes.Get(entity);
                ref var currentAnimationFrameComponent = ref currentFrames.Get(entity);
                
                currentAnimationFrameComponent.currentFrameHit = false;
                
                if (animationComponent.currentAnimation != currentAnimationFrameComponent.animation)
                {
                    var asset = animationComponent.animationsAsset;
                    var animationDefinition = asset.animations[animationComponent.currentAnimation];

                    currentAnimationFrameComponent.cancellationTime =
                        animationDefinition.GetDuration(animationComponent.fps);
                    
                    if (animationComponent.metadata != null)
                    {
                        // search for last defined hit box in animation
                        for (var i = 0; i < animationDefinition.TotalFrames; i++)
                        {
                            var metadata =
                                animationComponent.metadata.GetFrameMetadata(animationDefinition.frames[i].sprite);
                            if (metadata != null && metadata.hitBoxes.Count > 0)
                            {
                                currentAnimationFrameComponent.cancellationTime = (i + 1) / animationComponent.fps;
                            }
                        }
                    }
                }
                
                if (animationComponent.currentAnimation != currentAnimationFrameComponent.animation || animationComponent.currentFrame != currentAnimationFrameComponent.frame)
                {
                    currentAnimationFrameComponent.currentFrameHit = hitBox.hit.size.sqrMagnitude > Mathf.Epsilon;
                    currentAnimationFrameComponent.animation = animationComponent.currentAnimation;
                    currentAnimationFrameComponent.frame = animationComponent.currentFrame;
                }
            }
        }
    }
}