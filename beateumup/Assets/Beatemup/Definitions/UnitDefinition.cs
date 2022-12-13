using System.Collections.Generic;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Gemserk.Leopotam.Gameplay.Controllers;
using MyBox;
using UnityEngine;
using LookingDirection = Beatemup.Ecs.LookingDirection;
using TargetComponent = Beatemup.Ecs.TargetComponent;
using TargetingUtils = Beatemup.Ecs.TargetingUtils;

namespace Beatemup.Definitions
{
    public class UnitDefinition : MonoBehaviour, IEntityDefinition
    {
        public enum HealthType
        {
            Normal = 0,
            None = 1
        }

        public enum TargetType
        {
            None = 0,
            Enabled = 1
        }
        
        public enum MovementType
        {
            None = 0,
            Basic = 1
        }
        
        public enum ObstacleType
        {
            None = 0,
            Circle = 1,
            Box = 2
        }
        
        [Separator("Health")]
        public HealthType healthType = HealthType.Normal;
        [ConditionalField(nameof(healthType), false, HealthType.Normal)]
        public int hitPoints = 10;

        [Separator("Target")]
        public TargetType targetType = TargetType.Enabled;
        
        [Separator("Movement")] 
        public MovementType movementType = MovementType.Basic;

        [Separator("Model")]
        public bool hasModel = true;
        [ConditionalField(nameof(hasModel))]
        public GameObject modelPrefab;
        [ConditionalField(nameof(hasModel))]
        public bool hasShadow = true;
        [ConditionalField(nameof(hasModel))] 
        public Texture2D[] remapTexturesPerPlayer; 

        [Separator("Animation")]
        public bool hasAnimation = true;
        [ConditionalField(nameof(hasAnimation))]
        public AnimationsAsset animationsAsset;
        [ConditionalField(nameof(hasAnimation))]
        public HitboxAsset defaultHurtBoxAsset;
        [ConditionalField(nameof(hasAnimation))]
        public SpritesMetadata spritesMetadata;
        
        [Separator("States")]
        public bool hasStates = true;        
        
        [Separator("Controller")]
        public bool hasController;
        [ConditionalField(nameof(hasController))]
        public GameObject controllerObject;
        
        [Separator("Gravity")]
        public bool hasGravity = true;
        [ConditionalField(nameof(hasGravity))]
        public bool gravityStartsDisabled = false;
        [ConditionalField(nameof(hasGravity))]
        public float gravityScale = 1;
        
        [Separator("Obstacle")]
        public ObstacleType obstacleType = ObstacleType.None;
        [ConditionalField(nameof(obstacleType), false, ObstacleType.Circle, ObstacleType.Box)]
        public float obstacleSize = 0.25f;
        [ConditionalField(nameof(obstacleType), false, ObstacleType.Circle, ObstacleType.Box)]
        public bool obstacleIsStatic;
        
        [Separator("KillCount")]
        public bool hasKillCount = false;

        [Separator("Others")]
        public bool isVfx;
        public float jumpSpeed = 1;

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

            if (hasStates)
            {
                world.AddComponent(entity, StatesComponent.Create());
            }
            
            if (hasModel)
            {
                world.AddComponent(entity, new UnitModelComponent
                {
                    prefab = modelPrefab,
                    hasShadow = hasShadow,
                    remapTexturesPerPlayer = remapTexturesPerPlayer, 
                    color = Color.white
                });
                world.AddComponent(entity, new ModelShakeComponent()
                {
                    
                });
            }

            if (movementType == MovementType.Basic)
            {
                world.AddComponent(entity, new HorizontalMovementComponent
                {
                    speedMultiplier = 1.0f
                });
                world.AddComponent(entity, new VerticalMovementComponent()
                {
                    speed = 0
                });
            }
            
            if (hasGravity)
            {
                world.AddComponent(entity, new GravityComponent()
                {
                    disabled = gravityStartsDisabled,
                    scale = gravityScale
                });
            }
            
            world.AddComponent(entity, new JumpComponent
            {
                upSpeed = jumpSpeed
            });

            if (hasAnimation)
            {
                world.AddComponent(entity, new AnimationComponent
                {
                    animationsAsset = animationsAsset,
                    metadata = spritesMetadata,
                    currentAnimation = AnimationComponent.NoAnimation,
                    currentFrame = 0,
                    currentTime = 0,
                    state = AnimationComponent.State.Completed,
                    loops = 0,
                    paused = false
                });
                world.AddComponent(entity, new CurrentAnimationAttackComponent());
                
                if (spritesMetadata != null)
                {
                    world.AddComponent(entity, new HitBoxComponent
                    {
                        defaultHurt = defaultHurtBoxAsset
                    });
                }
            }
            
            if (targetType == TargetType.Enabled)
            {
                world.AddComponent(entity, new TargetComponent
                {
                    target = new TargetingUtils.Target
                    {
                        entity = entity
                    }
                });
            }
            
            if (healthType != HealthType.None)
            {
                world.AddComponent(entity, new HitPointsComponent
                {
                    total = hitPoints,
                    current = hitPoints,
                    hits = new List<HitData>()
                });
            }

            if (isVfx)
            {
                world.AddComponent(entity, new VfxComponent());
            }

            if (hasController)
            {
                world.AddComponent(entity, new ControllerComponent
                {
                    prefab = controllerObject
                });
            }

            if (obstacleType != ObstacleType.None)
            {
                world.AddComponent(entity, new ObstacleComponent()
                {
                    size = obstacleSize,
                    obstacleType = obstacleType,
                    isStatic = obstacleIsStatic
                });
            }

            if (hasKillCount)
            {
                world.AddComponent(entity, new KillCountComponent());
            }
            
            // world.AddComponent(entity, new QueryComponent()
            // {
            //     
            // });
        }
    }
}