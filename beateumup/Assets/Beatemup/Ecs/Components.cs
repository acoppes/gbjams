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
        public bool isControlled;
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
        public const float DefaultBufferTime = 0.2f;
        
        public const int MaxBufferCount = 15;
        
        public Vector2 direction;

        public Vector3 direction3d => new Vector3(direction.x, 0, direction.y);
        
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

        public void InsertInBuffer(string action)
        {
            buffer.Add(action);
            bufferTime = DefaultBufferTime;
        }

        public void ConsumeBuffer()
        {
            buffer.Clear();
        }
    }
    
    public struct LookingDirection : IEntityComponent
    {
        public Vector2 value;
    }

    public struct UnitModelComponent : IEntityComponent
    {
        public enum Visiblity
        {
            Visible = 0,
            Hidden = 1
        }
        
        public enum RotationType
        {
            FlipToLookingDirection = 0,
            Rotate = 1
        }
        
        public GameObject prefab;
        public Model instance;

        public RotationType rotation;

        public Visiblity visiblity;

        public bool IsVisible => visiblity == Visiblity.Visible;

        public bool hasShadow;
        public float shadowPerspective;
        
        public Texture2D[] remapTexturesPerPlayer;

        public Color color;
    }

    public struct ModelShakeComponent : IEntityComponent
    {
        public float duration;
        public float time;

        public float updateTime;

        public bool restart;
        
        public Vector3 currentOffset;

        public float intensity;

        public void Shake(float t, float intensity = 1)
        {
            duration = t;
            time = 0;
            restart = true;
            this.intensity = intensity;
        }
    }
    
    public struct HorizontalMovementComponent : IEntityComponent
    {
        public float speedMultiplier;

        public float baseSpeed;
        public float speed;
        
        public Vector3 currentVelocity;
        public Vector3 movingDirection;
    }

    public struct GravityComponent : IEntityComponent
    {
        public bool disabled;
        public float scale;
        public bool inContactWithGround;
    }

    public struct CurrentAnimationAttackComponent : IEntityComponent
    {
        public int animation;
        public int frame;
        
        public bool currentFrameHit;
        public float cancellationTime;
    }

    public struct HitData
    {
        public Vector3 position;
        public int hitPoints;
        public bool knockback;
        public Entity source;
    }

    public struct HitPointsComponent : IEntityComponent
    {
        [Flags]
        public enum AliveType
        {
            None = 0,
            Alive = 1 << 1,
            Death = 1 << 2
        }
        
        public int total;
        public int current;

        public AliveType aliveType => current > 0 ? AliveType.Alive : AliveType.Death;
        
        public List<HitData> hits;
        public event Action<World, Entity, HitPointsComponent> OnHitEvent;

        public void OnHit(World world, Entity entity)
        {
            OnHitEvent?.Invoke(world, entity, this);
        }
    }

    public struct VfxComponent : IEntityComponent
    {
        public float delay;
    }

    public struct DestroyableComponent : IEntityComponent
    {
        public bool destroy;
    }

    public struct TargetComponent : IEntityComponent
    {
        public TargetingUtils.Target target;
    }
    
    public struct PhysicsComponent : IEntityComponent
    {
        public enum ShapeType
        {
            None = 0,
            Circle = 1,
            Box = 2
        }
        
        public enum SyncType
        {
            Both = 0,
            FromPhysics = 1,
            // ToPhyiscs = 2
        }
        
        public ShapeType shapeType;

        public SyncType syncType;
        
        public bool disableCollideWithObstacles;
        public float size;
        // public int priority;

        public GameObject gameObject;
        public Rigidbody body;
        
        public Collider obstacleCollider;
        public Collider collideWithStaticCollider;

        public bool isStatic;

        public Vector3 velocity;
    }

    public struct KillCountComponent : IEntityComponent
    {
        public int count;
    }
}