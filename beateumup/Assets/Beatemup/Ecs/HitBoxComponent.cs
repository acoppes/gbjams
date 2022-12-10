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

        public Vector3 size3d => new Vector3(size.x, size.y, depth);

        public static HitBox AllTheWorld => new HitBox()
        {
            position = Vector2.zero,
            depth = Mathf.Infinity,
            offset = Vector2.zero,
            size = Vector2.positiveInfinity
        };
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
        public BoxCollider collider;
        public TargetReference targetReference;
    }
}