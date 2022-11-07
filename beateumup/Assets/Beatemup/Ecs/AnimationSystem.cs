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
                ref var animator = ref animations.Get(entity);

                if (animator.paused) 
                    return;

                if (animator.state == AnimationComponent.State.Playing)
                {
                    // if (animator.onStartEventPending)
                    // {
                    //     animator.OnStart();
                    //     animator.onStartEventPending = false;
                    // }
                    
                    var definition = animator.animationsAsset.animations[animator.currentAnimation];

                    var frameTime = 1.0f / definition.fps;
                        
                    animator.currentTime += Time.deltaTime;

                    while (animator.currentTime >= frameTime)
                    {
                        // if (definition.frames != null && definition.frames.Count > 0 && definition.frames[animator.currentFrame].hasEvent)
                        // {
                        //     animator.OnEvent();
                        // }
                        
                        animator.currentTime -= frameTime;
                        animator.currentFrame++;

                        if (animator.currentFrame >= definition.TotalFrames)
                        {
                            if (animator.loops > 0)
                            {
                                animator.loops -= 1;
                            }

                            // if (animator.loops == -1)
                            // {
                            //     animator.OnCompletedLoop();
                            // }
                            
                            if (animator.loops == 0)
                            {
                                animator.state = AnimationComponent.State.Completed;
                                animator.currentFrame = definition.TotalFrames - 1;
                                // animator.OnComplete();
                                break;
                            }
                            
                            animator.currentFrame = 0;
                        }
                    }
                }
            }
        }
    }
}