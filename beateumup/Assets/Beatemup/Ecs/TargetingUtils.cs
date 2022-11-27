using System.Collections.Generic;
using Beatemup.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace Beatemup.Ecs
{
    public static class TargetingUtils
    {
        public struct TargetingParameters
        {
            public int player;
            public HitBox area;
        }
        
        public class Target
        {
            public Entity entity;
        }
        
        private static readonly ContactFilter2D HurtBoxContactFilter = new()
        {
            useTriggers = true,
            useLayerMask = true,
            layerMask = LayerMask.GetMask("HurtBox")
        };
        
        private static readonly List<Collider2D> colliders = new ();

        public static List<Target> GetTargets(World world, Entity source)
        {
            var hitBox = world.GetComponent<HitBoxComponent>(source);
            var player = world.GetComponent<PlayerComponent>(source);
            
            return GetTargets(world, new TargetingParameters
            {
                player = player.player,
                area= hitBox.hit
            });
        }
        
        public static List<Target> GetTargets(World world, TargetingParameters targetingParameters)
        {
            var hitTargets = new List<Target>();
            
            var area = targetingParameters.area;

            colliders.Clear();

            if (Physics2D.OverlapBox(area.position + area.offset, area.size, 0, HurtBoxContactFilter,
                    colliders) > 0)
            {
                foreach (var collider in colliders)
                {
                    var targetEntityReference = collider.GetComponent<TargetReference>();
                    
                    var targetPlayer = world.GetComponent<PlayerComponent>(targetEntityReference.target.entity);

                    if (targetingParameters.player == targetPlayer.player)
                    {
                        continue;
                    }
                    
                    var targetHitBox = world.GetComponent<HitBoxComponent>(targetEntityReference.target.entity);
                    
                    if (Mathf.Abs(area.position.y - targetHitBox.hurt.position.y) >
                        (area.depth + targetHitBox.hurt.depth))
                    {
                        continue;
                    }

                    hitTargets.Add(targetEntityReference.target);
                }
            }

            return hitTargets;
        }
    }
}