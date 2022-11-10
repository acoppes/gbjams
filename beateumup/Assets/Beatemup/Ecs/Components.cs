using System;
using System.Collections.Generic;
using Beatemup.Models;
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

        public override string ToString()
        {
            return $"{name}:{isPressed}";
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

        public bool HasBufferedAction(Button button)
        {
            return HasBufferedActions(button.name);
        }

        public bool HasBufferedActions(params string[] actions)
        {
            if (actions.Length == 0)
            {
                return false;
            }

            var bufferStart = buffer.Count - actions.Length;
            
            if (bufferStart < 0)
            {
                return false;
            }

            for (var i = 0; i < actions.Length; i++)
            {
                var action = actions[i];
                if (!buffer[bufferStart + i].Equals(action, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        public void ConsumeBuffer()
        {
            buffer.Clear();
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
        public Model instance;

        public bool rotateToDirection;

        public Visiblity visiblity;

        public bool IsVisible => visiblity == Visiblity.Visible;

        public bool hasShadow;
    }
    
    public struct UnitMovementComponent : IEntityComponent
    {
        public bool disabled;
        public float speed;
        public Vector2 extraSpeed;
        public Vector2 currentVelocity;
        public Vector2 movingDirection;
    }
    
    public struct CurrentAnimationFrameComponent : IEntityComponent
    {
        public int animation;
        public int frame;
        
        public bool hit;
    }

    [Serializable]
    public struct HitBox
    {
        public const float DefaultDepth = 0.5f;
        
        [NonSerialized]
        public Vector2 position;
        public Vector2 offset;
        public Vector2 size;
    }
    
    public struct HitBoxComponent : IEntityComponent
    {
        public HitBox defaultHit;
        public HitBox defaultHurt;
        
        public HitBox hit;
        public HitBox hurt;
        
        public float depth;
        
        // hurt box collider2d
        public ColliderEntityReference instance;

        public GameObject debugHitBox;
        public GameObject debugHurtBox;
        public GameObject debugDepthBox;
    }

    public struct HitComponent : IEntityComponent
    {
        public int hits;
        public event Action<HitComponent> OnHitEvent;

        public void OnHit()
        {
            OnHitEvent?.Invoke(this);
        }
    }
}