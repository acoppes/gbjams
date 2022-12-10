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
        
        public static bool HasAliveFlag(this HitPointsComponent.AliveType self, HitPointsComponent.AliveType flag)
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

        public struct RuntimeTargetingParameters
        {
            public enum CheckAreaType
            {
                Nothing = 0,
                HitBox = 1
            }
            
            // searcher properties
            public int player;
            
            // filter properties
            public HitBox area;
            public CheckAreaType checkAreaType;
            
            public PlayerAllianceType playerAllianceType;
            public string name;

            public HitPointsComponent.AliveType aliveType;
        }
        
        public class Target
        {
            public Entity entity;
            public int player;
            public Vector3 position;
            public HitBox hurtBox;
            public string name;
            public HitPointsComponent.AliveType aliveType = HitPointsComponent.AliveType.None;
        }
        
        private static readonly ContactFilter2D HurtBoxContactFilter = new()
        {
            useTriggers = true,
            useLayerMask = true,
            layerMask = LayerMask.GetMask("HurtBox")
        };
        
        private static readonly Collider[] colliders = new Collider[20];

        public static List<Target> GetTargets(this World world, Entity source)
        {
            var hitBox = world.GetComponent<HitBoxComponent>(source);
            var player = world.GetComponent<PlayerComponent>(source);
            
            return world.GetTargets(new RuntimeTargetingParameters
            {
                player = player.player,
                area = hitBox.hit,
                playerAllianceType = PlayerAllianceType.Enemies,
                checkAreaType = RuntimeTargetingParameters.CheckAreaType.HitBox,
                aliveType = HitPointsComponent.AliveType.Alive
            });
        }
        
        public static List<Target> GetTargets(this World world, RuntimeTargetingParameters runtimeTargetingParameters)
        {
            var result = new List<Target>();
            var targets = new List<Target>();
            
            if (runtimeTargetingParameters.checkAreaType == RuntimeTargetingParameters.CheckAreaType.HitBox)
            {
                // collect targets using physics collider
                var area = runtimeTargetingParameters.area;

                var debugBoxObject = new GameObject("~DebugBoxObject");
                var boxCollider = debugBoxObject.AddComponent<BoxCollider>();
                boxCollider.transform.position = area.position3d;
                boxCollider.size = area.size3d;
                debugBoxObject.layer = LayerMask.NameToLayer("Ignore Raycast");

                var colliderCount = Physics.OverlapBoxNonAlloc(area.position3d, area.size3d * 0.5f, colliders,
                    Quaternion.identity, HurtBoxContactFilter.layerMask,
                    QueryTriggerInteraction.Collide);

                if (colliderCount > 0)
                {
                    for (var i = 0; i < colliderCount; i++)
                    {
                        var collider = colliders[i];
                        var targetEntityReference = collider.GetComponent<TargetReference>();
                        var target = targetEntityReference.target;

                        // check hitbox depth
                        // if (!area.IsInsideDepth(target.hurtBox))
                        // {
                        //     continue;
                        // }

                        targets.Add(target);
                    }
                }
            } else
            {
                var targetComponents = world.GetComponents<TargetComponent>();
                
                foreach (var entity in world.GetFilter<TargetComponent>().End())
                {
                    var targetComponent = targetComponents.Get(entity);
                    targets.Add(targetComponent.target);
                }
            }
            
            // filter valid targets
            foreach (var target in targets)
            {
                if (!runtimeTargetingParameters.playerAllianceType.CheckPlayerAlliance(runtimeTargetingParameters.player, target.player))
                {
                    continue;
                }
                
                if (runtimeTargetingParameters.name != null && 
                    !runtimeTargetingParameters.name.Equals(target.name, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (runtimeTargetingParameters.aliveType != HitPointsComponent.AliveType.None)
                {
                    if (target.aliveType == HitPointsComponent.AliveType.None)
                    {
                        continue;
                    }
                    
                    if (!runtimeTargetingParameters.aliveType.HasFlag(target.aliveType))
                    {
                        continue;
                    }
                }

                result.Add(target);
            }

            return result;
        }
    }
}