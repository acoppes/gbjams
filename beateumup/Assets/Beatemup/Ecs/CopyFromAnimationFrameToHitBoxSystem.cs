using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class CopyFromAnimationFrameToHitBoxSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var animations = world.GetComponents<AnimationComponent>();
            var hitBoxes = world.GetComponents<HitBoxComponent>();
            var positions = world.GetComponents<PositionComponent>();
            var lookingDirections = world.GetComponents<LookingDirection>();
            
            foreach (var entity in world.GetFilter<AnimationComponent>()
                         .Inc<HitBoxComponent>()
                         .Inc<PositionComponent>()
                         .Inc<LookingDirection>().End())
            {
                var animationComponent = animations.Get(entity);
                var position = positions.Get(entity);
                var lookingDirection = lookingDirections.Get(entity);
                
                ref var hitBox = ref hitBoxes.Get(entity);
                
                var asset = animationComponent.animationsAsset;
                var animation = asset.animations[animationComponent.currentAnimation];
                var frame = animation.frames[animationComponent.currentFrame];
                
                hitBox.hurt = new HitBox
                {
                    size = hitBox.hurt.size,
                    position = new Vector2(position.value.x, position.value.y),
                    depth = hitBox.hurt.depth
                };

                if (frame.hitbox == null)
                {
                    hitBox.hit = new HitBox
                    {
                        position = Vector2.zero,
                        size = Vector2.zero,
                        depth = 0
                    };
                }
                else
                {
                    var offset = frame.hitbox.offset;
                    
                    if (lookingDirection.value.x < 0)
                    {
                        offset.x *= -1;
                    }
                    
                    hitBox.hit = new HitBox
                    {
                        size = frame.hitbox.size,
                        position = new Vector2(position.value.x, position.value.y) + offset,
                        depth = frame.hitbox.depth
                    };
                }
            }
        }
    }
}