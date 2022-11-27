using Beatemup.Development;
using Beatemup.Models;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Beatemup.Ecs
{
    public struct HitBox
    {
        public Vector2 position;
        public Vector2 offset;
        public Vector2 size;
        public float depth;
    }
    
    public struct HitBoxComponent : IEntityComponent
    {
        public HitboxAsset defaultHurt;
        
        public HitBox hit;
        public HitBox hurt;

        public DebugHitBox debugHitBox;
        public DebugHitBox debugHurtBox;
    }
    
    public struct HurtBoxColliderComponent : IEntityComponent
    {
        public BoxCollider2D collider;
        public TargetReference targetReference;
    }
}