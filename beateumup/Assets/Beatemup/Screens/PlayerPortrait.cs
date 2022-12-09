using System;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Beatemup.Screens
{
    public class PlayerPortrait : MonoBehaviour
    {
        [NonSerialized]
        public World world;
        
        [NonSerialized]
        private Entity entity = Entity.NullEntity;

        public string playerEntityName;

        public CanvasGroup canvasGroup;

        public Image healthBarCurrent;

        public void LateUpdate()
        {
            if (entity == Entity.NullEntity)
            {
                var targets = world.GetTargets(new TargetingUtils.RuntimeTargetingParameters()
                {
                    checkAreaType = TargetingUtils.RuntimeTargetingParameters.CheckAreaType.Nothing,
                    name = playerEntityName,
                    aliveType = HitPointsComponent.AliveType.Alive,
                    player = 0,
                    playerAllianceType = TargetingUtils.PlayerAllianceType.Everything
                });

                if (targets.Count > 0)
                {
                    entity = targets[0].entity;
                }
            }
            
            healthBarCurrent.fillAmount = 0;
            
            // canvasGroup.alpha = entity == Entity.NullEntity ? 0 : 1;

            if (entity != Entity.NullEntity)
            {
                if (world.HasComponent<HitPointsComponent>(entity))
                {
                    var hitPoints = world.GetComponent<HitPointsComponent>(entity);
                    healthBarCurrent.fillAmount = (float)hitPoints.current / hitPoints.total;

                    if (hitPoints.current == 0)
                    {
                        entity = Entity.NullEntity;
                    }
                }
            }
            
            // update bar, etc
        }
    }
}