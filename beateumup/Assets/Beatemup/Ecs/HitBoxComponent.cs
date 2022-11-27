using System.Collections.Generic;
using Beatemup.Development;
using Beatemup.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
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

    public static class HitBoxUtils
    {
        private static readonly ContactFilter2D HurtBoxContactFilter = new()
        {
            useTriggers = true,
            useLayerMask = true,
            layerMask = LayerMask.GetMask("HurtBox")
        };
        
        private static readonly List<Collider2D> colliders = new ();

        public static HitBox GetHitBox(this HitboxAsset hitBoxAsset, PositionComponent position, LookingDirection lookingDirection)
        {
            var offset = hitBoxAsset.offset;
                    
            if (lookingDirection.value.x < 0)
            {
                offset.x *= -1;
            }
                    
            return new HitBox
            {
                size = hitBoxAsset.size,
                position = new Vector2(position.value.x, position.value.y),
                offset = offset + new Vector2(0, position.value.z), 
                depth = hitBoxAsset.depth
            };
        }
        
        public static List<Entity> GetTargets(World world, Entity source)
        {
            var hitBox = world.GetComponent<HitBoxComponent>(source);
            return GetTargets(world, source, hitBox.hit);
        }
        
        public static List<Entity> GetTargets(World world, Entity source, HitBox hit)
        {
            var hitBox = world.GetComponent<HitBoxComponent>(source);
            var player = world.GetComponent<PlayerComponent>(source);

            var hitTargets = new List<Entity>();

            colliders.Clear();

            if (Physics2D.OverlapBox(hit.position + hit.offset, hit.size, 0, HurtBoxContactFilter,
                    colliders) > 0)
            {
                foreach (var collider in colliders)
                {
                    var entityReference = collider.GetComponent<ColliderEntityReference>();
                    
                    var targetPlayer = world.GetComponent<PlayerComponent>(entityReference.entity);

                    if (player.player == targetPlayer.player)
                    {
                        continue;
                    }
                    
                    var targetHitBox = world.GetComponent<HitBoxComponent>(entityReference.entity);
                    
                    if (Mathf.Abs(hit.position.y - targetHitBox.hurt.position.y) >
                        (hit.depth + targetHitBox.hurt.depth))
                    {
                        continue;
                    }

                    hitTargets.Add(entityReference.entity);
                }
            }

            return hitTargets;
        }
    }
}