using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9.Controllers
{
    public class PickupController : MonoBehaviour
    {
        public void OnPickup(UnitComponent unit)
        {
            var pickup = GetComponent<PickupComponent>();

            if (pickup.pickupType.Equals("sword"))
            {
                // swap unit to sword attack
            } else if (pickup.pickupType.Equals("kunai"))
            {
                // swap attack to kunai
            } else if (pickup.pickupType.Equals("coin"))
            {
                unit.inventoryComponent.coins += pickup.count;
            } else if (pickup.pickupType.Equals("sushi"))
            {
                unit.health.current += pickup.count;
                if (unit.health.current > unit.health.total)
                    unit.health.current = unit.health.total;
            } else if (pickup.pickupType.Equals("trap"))
            {
                unit.health.current -= pickup.count;
            }
        }
    }
}
