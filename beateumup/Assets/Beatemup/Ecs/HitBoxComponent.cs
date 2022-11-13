using System;
using System.Collections.Generic;
using Beatemup.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace Beatemup.Ecs
{
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

    public static class HitBoxUtils
    {
        private static readonly ContactFilter2D HurtBoxContactFilter = new()
        {
            useTriggers = true,
            useLayerMask = true,
            layerMask = LayerMask.GetMask("HurtBox")
        };
        
        private static readonly List<Collider2D> colliders = new ();
        
        
        public static List<Entity> GetTargets(World world, Entity source)
        {
            var hitBox = world.GetComponent<HitBoxComponent>(source);
            var player = world.GetComponent<PlayerComponent>(source);

            var hitTargets = new List<Entity>();

            colliders.Clear();

            if (Physics2D.OverlapBox(hitBox.hit.position + hitBox.hit.offset, hitBox.hit.size, 0, HurtBoxContactFilter,
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
                    
                    if (Mathf.Abs(hitBox.hit.position.y - targetHitBox.hurt.position.y) >
                        (hitBox.depth + targetHitBox.depth))
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