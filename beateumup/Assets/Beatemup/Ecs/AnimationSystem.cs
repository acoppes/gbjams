using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class AnimationSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var animations = world.GetComponents<AnimationComponent>();
            
            foreach (var entity in world.GetFilter<AnimationComponent>().End())
            {
                ref var animationComponent = ref animations.Get(entity);

                if (animationComponent.paused) 
                    return;

                if (animationComponent.state == AnimationComponent.State.Playing)
                {
                    // if (animator.onStartEventPending)
                    // {
                    //     animator.OnStart();
                    //     animator.onStartEventPending = false;
                    // }
                    
                    var definition = animationComponent.animationsAsset.animations[animationComponent.currentAnimation];

                    var frameTime = 1.0f / definition.fps;
                        
                    animationComponent.currentTime += Time.deltaTime;

                    while (animationComponent.currentTime >= frameTime)
                    {
                        // if (definition.frames != null && definition.frames.Count > 0 && definition.frames[animator.currentFrame].hasEvent)
                        // {
                        //     animator.OnEvent();
                        // }
                        
                        animationComponent.currentTime -= frameTime;
                        animationComponent.currentFrame++;

                        if (animationComponent.currentFrame >= definition.TotalFrames)
                        {
                            if (animationComponent.loops > 0)
                            {
                                animationComponent.loops -= 1;
                            }

                            // if (animator.loops == -1)
                            // {
                            //     animator.OnCompletedLoop();
                            // }
                            
                            if (animationComponent.loops == 0)
                            {
                                animationComponent.state = AnimationComponent.State.Completed;
                                animationComponent.currentFrame = definition.TotalFrames - 1;
                                // animator.OnComplete();
                                break;
                            }
                            
                            animationComponent.currentFrame = 0;
                        }
                    }
                }
            }
        }
    }
}