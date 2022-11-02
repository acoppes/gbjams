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
        public const int DoubleTapFrames = 20;
        
        public int current;
        
        public bool[] pressedBuffer;

        public bool isPressed => pressedBuffer[current];

        private int lastPressedFrame;
        
        // public bool wasReleased;
        // public bool wasPressed;

        public bool doubleTap;

        public Button(int buffer)
        {
            pressedBuffer = new bool[buffer];
            current = 0;

            lastPressedFrame = 0;
            doubleTap = false;

            // wasPressed = false;
            // wasReleased = false;
        }

        public void UpdatePressed(bool pressed)
        {
            var wasPressed = !pressedBuffer[current] && pressed;
            // wasReleased = pressedBuffer[current] && !pressed;
            
            current++;
            lastPressedFrame--;
            
            if (current >= pressedBuffer.Length)
            {
                current = 0;
            }
            
            pressedBuffer[current] = pressed;

            if (wasPressed)
            {
                doubleTap = lastPressedFrame > 0;
                lastPressedFrame = DoubleTapFrames;
            }
        }

        public static Button Default()
        {
            return new Button(8);
        }
    }
    
    public struct ControlComponent : IEntityComponent
    {
        public Vector2 direction;
        
        public Button up;
        public Button down;
        
        public Button forward;
        public Button backward;
        
        public Button button1;
        public Button button2;

        public static ControlComponent Default()
        {
            return new ControlComponent()
            {
                up = Button.Default(),
                down = Button.Default(),
                forward = Button.Default(),
                backward = Button.Default(),
                button1 = Button.Default(),
                button2 = Button.Default()
            };
        }
    }
    
    public struct LookingDirection : IEntityComponent
    {
        public Vector2 value;
        public bool locked;
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
        public bool sprinting;
        
        public bool attack;
        public bool attackMoving;

        public StateTriggers stateTriggers;

        public bool disableAutoUpdate;
    }

    public struct AnimatorComponent : IEntityComponent
    {
        public Animator animator;
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