using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9.Controllers
{
    public class RoomMainMenuController : MonoBehaviour
    {
        public GameObject swordPickupPrefab;
        public GameObject kunaiPickupPrefab;

        public Entity swordPickup;
        public Entity kunaiPickup;

        private Vector2 swordPosition;
        private Vector2 kunaiPosition;
        
        private World world;
        
        private void Awake()
        {
            world = FindObjectOfType<World>();

            swordPosition = swordPickup.transform.position;
            kunaiPosition = kunaiPickup.transform.position;
        }

        private void OnEnable()
        {
            if (world != null)
            {
                world.onPickup += OnPickup;
            }
        }

        private void OnDisable()
        {
            if (world != null)
            {
                world.onPickup -= OnPickup;
            }
        }

        private void OnRoomStart(World world)
        {
            // get main player, remove default weapon...

            var mainUnitList = world.GetEntityList<MainUnitComponent>();
            foreach (var mainUnit in mainUnitList)
            {
                var attack = mainUnit.GetComponent<AttackComponent>();
                attack.weaponData = null;
            }
        }

        private void OnPickup(Entity pickupEntity)
        {
            if (pickupEntity == swordPickup && kunaiPickup == null)
            {
                var kunaiObject = GameObject.Instantiate(kunaiPickupPrefab, kunaiPosition, Quaternion.identity);
                kunaiPickup = kunaiObject.GetComponent<Entity>();
            } else if (pickupEntity == kunaiPickup && swordPickup == null)
            {
                var swordObject = GameObject.Instantiate(swordPickupPrefab, swordPosition, Quaternion.identity);
                swordPickup = swordObject.GetComponent<Entity>();
            }

            if (pickupEntity.pickup.pickupType.Equals("weapon"))
            {
                // activate exits
            }
        }
    }
}
