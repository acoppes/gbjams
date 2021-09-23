using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9.Controllers
{
    public class PickupController : MonoBehaviour
    {
        public void OnPickup(Entity entity)
        {
            var pickup = GetComponent<PickupComponent>();

            if (pickup.pickupType.Equals("sword"))
            {
                // swap unit to sword attack
                entity.attack.attackType = "sword";
            } else if (pickup.pickupType.Equals("kunai"))
            {
                // swap attack to kunai
                entity.attack.attackType = "sword";
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
