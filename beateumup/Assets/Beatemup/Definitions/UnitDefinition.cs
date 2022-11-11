using System.Collections.Generic;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;
using UnityEngine.Serialization;
using LookingDirection = Beatemup.Ecs.LookingDirection;

namespace Beatemup.Definitions
{
    public class UnitDefinition : MonoBehaviour, IEntityDefinition
    {
        public float movementSpeed;

        public GameObject modelPrefab;
        public bool hasShadow = true;

        public AnimationsAsset animationsAsset;
        [FormerlySerializedAs("animationMetadata")] 
        public AnimationHitboxMetadata animationHitboxMetadata;

        public HitBox defaultHurtBox;

        public bool isVfx;

        public void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new DestroyableComponent());
            world.AddComponent(entity, new PlayerComponent());
            world.AddComponent(entity, new PositionComponent());
            world.AddComponent(entity, new LookingDirection
            {
                value = Vector2.right
            });

            world.AddComponent(entity, ControlComponent.Default());

            world.AddComponent(entity, new StatesComponent());

            if (modelPrefab != null)
            {
                world.AddComponent(entity, new UnitModelComponent
                {
                    prefab = modelPrefab,
                    hasShadow = hasShadow
                });
            }

            world.AddComponent(entity, new UnitMovementComponent()
            {
                speed = movementSpeed
            });

            if (animationsAsset != null)
            {
                world.AddComponent(entity, new AnimationComponent
                {
                    fps = AnimationComponent.DefaultFrameRate,
                    animationsAsset = animationsAsset,
                    metadata = animationHitboxMetadata,
                    currentAnimation = 0,
                    currentFrame = 0,
                    currentTime = 0,
                    state = AnimationComponent.State.Completed,
                    loops = 0,
                    paused = false
                });
                world.AddComponent(entity, new CurrentAnimationFrameComponent());
                
                if (animationHitboxMetadata != null)
                {
                    world.AddComponent(entity, new HitBoxComponent
                    {
                        depth = HitBox.DefaultDepth,
                        defaultHurt = defaultHurtBox
                    });
                }
            }
            
            world.AddComponent(entity, new HitComponent()
            {
                hits = new List<HitData>()
            });

            if (isVfx)
            {
                world.AddComponent(entity, new VfxComponent());
            }
        }
    }
}