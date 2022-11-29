using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Beatemup.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace Beatemup.Ecs
{
    public static class TargetingUtils
    {
        [Flags]
        public enum PlayerAllianceType
        {
            Nothing = 0,
            Enemies = 1 << 0,
            Allies = 1 << 1,
            Everything = -1
        }
        
        public static bool HasAllianceFlag(this PlayerAllianceType self, PlayerAllianceType flag)
        {
            return (self & flag) == flag;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckPlayerAlliance(this PlayerAllianceType playerAllianceType, int p0, int p1)
        {
            // get if p0 and p1 are allies or enemies from some config?
            // for now we assume all players are enemies

            if (playerAllianceType == PlayerAllianceType.Everything)
                return true;

            var allies = p0 == p1;
            var enemies = !allies;

            if (allies && playerAllianceType.HasAllianceFlag(PlayerAllianceType.Allies))
                return true;
            
            if (enemies && playerAllianceType.HasAllianceFlag(PlayerAllianceType.Enemies))
                return true;

            return false;
        }

        public struct TargetingParameters
        {
            // searcher properties
            public int player;
            
            // filter properties
            public HitBox area;
            public PlayerAllianceType playerAllianceType;
            public string name;
        }
        
        public class Target
        {
            public Entity entity;
            public int player;
            public Vector3 position;
            public HitBox hurtBox;
            public string name;
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
            
            return GetTargets(new TargetingParameters
            {
                player = player.player,
                area = hitBox.hit,
                playerAllianceType = PlayerAllianceType.Enemies
            });
        }
        
        public static List<Target> GetTargets(TargetingParameters targetingParameters)
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
                    var target = targetEntityReference.target;
                    
                    if (!targetingParameters.playerAllianceType.CheckPlayerAlliance(targetingParameters.player, target.player))
                    {
                        continue;
                    }

                    // check hitbox depth
                    if (!area.IsInsideDepth(target.hurtBox))
                    {
                        continue;
                    }

                    if (targetingParameters.name != null && 
                        !targetingParameters.name.Equals(target.name, StringComparison.OrdinalIgnoreCase))
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