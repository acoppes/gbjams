using System;
using System.Collections.Generic;
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
        public string name;
        
        public bool isPressed;

        public bool wasPressedThisFrame;

        public Button(string name)
        {
            this.name = name;
            isPressed = false;
            wasPressedThisFrame = false;
        }

        public void UpdatePressed(bool pressed)
        {
            wasPressedThisFrame = !isPressed && pressed;
            isPressed = pressed;
        }
    }
    
    public struct ControlComponent : IEntityComponent
    {
        public const int MaxBufferCount = 15;
        
        public Vector2 direction;
        
        public Button right;
        public Button left;
        public Button up;
        public Button down;
        
        public Button forward;
        public Button backward;
        
        public Button button1;
        public Button button2;

        public List<string> buffer;
        public float bufferTime;

        public static ControlComponent Default()
        {
            return new ControlComponent()
            {
                right = new Button(nameof(right)),
                left = new Button(nameof(left)),
                up = new Button(nameof(up)),
                down = new Button(nameof(down)),
                forward = new Button(nameof(forward)),
                backward = new Button(nameof(backward)),
                button1 = new Button(nameof(button1)),
                button2 = new Button(nameof(button2)),
                buffer = new List<string>()
            };
        }

        public bool IsPreviousAction(Button button, int count)
        {
            return IsPreviousAction(button.name, count);
        }

        public bool IsPreviousAction(string actionName, int count)
        {
            for (var i = buffer.Count - 1; i >= 0; i--)
            {
                if (!buffer[i].Equals(actionName, StringComparison.OrdinalIgnoreCase))
                    return false;
                
                count--;
                
                if (count == 0)
                    return true;
            }

            return false;
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

        public Vector2 extraSpeed;

        public Vector2 currentVelocity;

        public Vector2 movingDirection;
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
        public bool attack2;
        public bool attackMoving;

        public StateTriggers stateTriggers;

        public bool disableAutoUpdate;
    }

    public struct AnimatorComponent : IEntityComponent
    {
        public Animator animator;
    }
}