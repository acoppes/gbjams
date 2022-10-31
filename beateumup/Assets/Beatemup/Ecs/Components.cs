using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Beatemup.Ecs
{
    public struct PlayerInputComponent : IEntityComponent
    {
        public bool disabled;
        public int playerInput;
    }

    public struct Button
    {
        public bool isPressed;
        public bool wasReleased;
        public bool wasPressed;

        public void UpdatePressed(bool pressed)
        {
            wasPressed = !isPressed && pressed;
            wasReleased = isPressed && !pressed;
            isPressed = pressed;
        }
    }
    
    public struct ControlComponent : IEntityComponent
    {
        public Vector2 direction;
        public Button button1;
    }

    public struct UnitModelComponent : IEntityComponent
    {
        public enum Visiblity
        {
            Visible = 0,
            Hidden = 1
        }
        
        public GameObject prefab;
        public GameObject instance;

        public Transform subModel;

        public bool rotateToDirection;

        public Visiblity visiblity;

        public bool IsVisible => visiblity == Visiblity.Visible;
    }
    
    public struct UnitMovementComponent : IEntityComponent
    {
        public bool disabled;
        
        public float speed;
        public float extraSpeed;

        public Vector2 currentVelocity;

        public Vector2 movingDirection;
        
        public float totalSpeed => speed + extraSpeed;
    }

    public struct KeepInsideCameraComponent : IEntityComponent
    {
        
    }
    
    public struct StateTriggers
    {
        public bool hit;
    }
    
    public struct ModelStateComponent : IEntityComponent
    {
        public bool walking;
        public bool up;
        public bool dashing;

        public StateTriggers stateTriggers;

        public bool disableAutoUpdate;
    }

    public struct AnimatorComponent : IEntityComponent
    {
        public Animator animator;
    }

    public class TargetExtra
    {
        public Vector2 lookingDirection;
        public bool isAlive;
    }
    public struct TerrainCollisionComponent : IEntityComponent
    {
        public Vector2 lastValidPosition;
    }

    public struct UnitTypeComponent : IEntityComponent
    {
        public int type;
    }
}