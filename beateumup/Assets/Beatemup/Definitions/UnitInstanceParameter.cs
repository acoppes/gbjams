using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using MyBox;
using UnityEngine;
using LookingDirection = Beatemup.Ecs.LookingDirection;

namespace Beatemup.Definitions
{
    public class UnitInstanceParameter : MonoBehaviour, IEntityInstanceParameter
    {
        public enum PositionType
        {
            None = 0,
            OverrideFromTransform = 1,
        }
        
        [Separator("Identify")] 
        public string entityName;
        [ConditionalField(nameof(entityName), true, "")]
        public bool singleton;

        [Separator("Location")] 
        public PositionType positionType = PositionType.OverrideFromTransform;
        [ConditionalField(nameof(positionType), false, PositionType.OverrideFromTransform)]
        public GamePerspectiveAsset gamePerspective;
        
        [Separator("Control")] 
        public bool controllable = false;
        [ConditionalField(nameof(controllable))]
        public int playerInput;

        [Separator("Player Team")] 
        public bool overridePlayer;
        [ConditionalField(nameof(overridePlayer))]
        public int team;
        
        [Separator("Looking Direction")] 
        public bool overrideLookingDirection = true;
        [ConditionalField(nameof(overrideLookingDirection))]
        public float startingLookingDirectionAngle = 0;

        [Separator("Animation")]
        public bool overrideAnimation;
        [ConditionalField(nameof(overrideAnimation))]
        public string startingAnimation;
        [ConditionalField(nameof(overrideAnimation))]
        public bool randomizeStartFrame;
        
        [Separator("HitPoints")]
        public bool overrideHitPoints;
        [ConditionalField(nameof(overrideHitPoints))]
        public int hitPoints;

        public void Apply(World world, Entity entity)
        {
            if (positionType == PositionType.OverrideFromTransform)
            {
                ref var position = ref world.GetComponent<PositionComponent>(entity);

                if (gamePerspective != null)
                {
                    position.value =
                        gamePerspective.ConvertToWorld(transform.position);
                }
                else
                {
                    position.value = new Vector3(transform.position.x, transform.position.y, 0);
                }
            }
            
            if (controllable)
            {
                world.AddComponent(entity, new PlayerInputComponent()
                {
                    playerInput = playerInput,
                    disabled = false
                });
            }

            if (overrideLookingDirection && world.HasComponent<LookingDirection>(entity))
            {
                ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
                lookingDirection.value = Vector2.right.Rotate(startingLookingDirectionAngle * Mathf.Deg2Rad);
            }
            
            if (overrideAnimation && world.HasComponent<AnimationComponent>(entity))
            {
                ref var animationComponent = ref world.GetComponent<AnimationComponent>(entity);
                var animation = animationComponent.animationsAsset.FindByName(startingAnimation);
                var totalFrames = animationComponent.animationsAsset.animations[animation].frames.Count;
                animationComponent.Play(animation, UnityEngine.Random.Range(0, totalFrames), -1);
            }

            if (overrideHitPoints && world.HasComponent<HitPointsComponent>(entity))
            {
                ref var hitPointsComponent = ref world.GetComponent<HitPointsComponent>(entity);
                hitPointsComponent.total = hitPoints;
                hitPointsComponent.current = hitPoints;
            }

            if (overridePlayer)
            {
                ref var playerComponent = ref world.GetComponent<PlayerComponent>(entity);
                playerComponent.player = team;
            }

            if (!string.IsNullOrEmpty(entityName))
            {
                world.AddComponent(entity, new NameComponent
                {
                    name = entityName,
                    singleton = singleton
                });
            }
        }

        private void OnValidate()
        {
            if (!gameObject.IsSafeToModifyName())
                return;
            
            if (!string.IsNullOrEmpty(entityName))
            {
                gameObject.name = $"Spawn({entityName})";
            }
            else
            {
                gameObject.name = $"Spawn()"; 
            }
        }
    }
}