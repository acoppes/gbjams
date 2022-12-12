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

        public Text killCountText;

        private int cachedKillCount;

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

            var isNullOrDeath = entity == Entity.NullEntity || !world.Exists(entity);
            
            canvasGroup.alpha = isNullOrDeath ? 0.25f : 1;

            if (!isNullOrDeath)
            {
                if (world.HasComponent<HitPointsComponent>(entity))
                {
                    var hitPoints = world.GetComponent<HitPointsComponent>(entity);
                    healthBarCurrent.fillAmount = (float)hitPoints.current / hitPoints.total;
                }
                
                if (killCountText != null && world.HasComponent<KillCountComponent>(entity))
                {
                    var killCountComponent = world.GetComponent<KillCountComponent>(entity);
                    if (killCountComponent.count != cachedKillCount)
                    {
                        killCountText.text = $"{killCountComponent.count}";
                        cachedKillCount = killCountComponent.count;
                    }
                }
            }
            
            // update bar, etc
        }
    }
}