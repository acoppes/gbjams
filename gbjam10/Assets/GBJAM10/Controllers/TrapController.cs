using GBJAM10.Components;
using UnityEngine;

namespace GBJAM10.Controllers
{
    public class TrapController : EntityController
    {
        private float damageCurrentTime;
        
        public override void OnWorldUpdate(World world)
        {
            damageCurrentTime -= Time.deltaTime;

            if (damageCurrentTime < 0)
            {
                // search for targets...

                var targets = world.GetEntitiesWith<HealthComponent>();

                foreach (var target in targets)
                {
                    if (entity.collider.collider.OverlapPoint(target.transform.position))
                    {
                        target.health.damages += entity.attack.weaponData.damage;
                    }
                }

                damageCurrentTime = entity.attack.weaponData.cooldown;
            }
        }
    }
}