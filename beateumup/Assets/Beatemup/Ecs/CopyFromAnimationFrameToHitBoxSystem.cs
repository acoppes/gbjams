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
            
            foreach (var entity in world.GetFilter<HitBoxComponent>().End())
            {
                ref var hitBox = ref hitBoxes.Get(entity);
                hitBox.hurt = hitBox.defaultHurt;
                hitBox.hit = hitBox.defaultHit;
            }

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

                hitBox.hurt.position = new Vector2(position.value.x, position.value.y);
                hitBox.hurt.offset += new Vector2(0, position.value.z);

                if (frame.hitBoxes.Count > 0)
                {
                    var hitbox = frame.hitBoxes[0];
                    
                    var offset = hitbox.offset;
                    
                    if (lookingDirection.value.x < 0)
                    {
                        offset.x *= -1;
                    }
                    
                    hitBox.hit = new HitBox
                    {
                        size = hitbox.size,
                        position = new Vector2(position.value.x, position.value.y),
                        offset = offset + new Vector2(0, position.value.z)
                    };
                }
            }
        }
    }
}