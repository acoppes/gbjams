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

        // hurt box collider2d
        public ColliderEntityReference instance;

        public DebugHitBox debugHitBox;
        public DebugHitBox debugHurtBox;
    }
}