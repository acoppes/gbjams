using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9.Controllers
{
    public class PickupController : MonoBehaviour
    {
        public void OnPickup(Entity entity)
        {
            var pickup = GetComponent<PickupComponent>();

            if (pickup.pickupType.Equals("weapon"))
            {
                // swap unit to sword attack
                entity.attack.weaponData = pickup.pickupData as WeaponData;
            } else if (pickup.pickupType.Equals("coin"))
            {
                entity.inventory.coins += pickup.count;
            } else if (pickup.pickupType.Equals("sushi"))
            {
                entity.health.current += pickup.count;
                if (entity.health.current > entity.health.total)
                    entity.health.current = entity.health.total;
            } else if (pickup.pickupType.Equals("trap"))
            {
                entity.health.current -= pickup.count;
            }
        }
    }
}
