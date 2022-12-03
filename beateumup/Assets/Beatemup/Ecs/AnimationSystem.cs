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
                {
                    continue;
                }

                if (animationComponent.pauseTime > 0)
                {
                    animationComponent.pauseTime -= Time.deltaTime;
                    continue;
                }

                if (animationComponent.state == AnimationComponent.State.Playing)
                {
                    // if (animationComponent.onStartEventPending)
                    // {
                    //     animationComponent.OnStart();
                    //     animationComponent.onStartEventPending = false;
                    // }
                    
                    var definition = animationComponent.animationsAsset.animations[animationComponent.currentAnimation];

                    var frameTime = 1.0f / definition.fps;
                        
                    animationComponent.currentTime += Time.deltaTime;
                    animationComponent.playingTime += Time.deltaTime;

                    while (animationComponent.currentTime >= frameTime)
                    {
                        // if (definition.frames != null && definition.frames.Count > 0 && definition.frames[animationComponent.currentFrame].HasEvents)
                        // {
                        //     animationComponent.OnEvent();
                        // }
                        
                        animationComponent.currentTime -= frameTime;
                        animationComponent.currentFrame++;

                        if (animationComponent.currentFrame >= definition.TotalFrames)
                        {
                            if (animationComponent.loops > 0)
                            {
                                animationComponent.loops -= 1;
                            }

                            // if (animationComponent.loops == -1)
                            // {
                            //     animationComponent.OnCompletedLoop();
                            // }
                            
                            if (animationComponent.loops == 0)
                            {
                                animationComponent.state = AnimationComponent.State.Completed;
                                animationComponent.currentFrame = definition.TotalFrames - 1;
                                // animationComponent.OnComplete();
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