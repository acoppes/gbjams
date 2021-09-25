using System.Linq;
using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9.Controllers
{
    public class RoomMainMenuController : EntityController
    {
        public GameObject swordPickupPrefab;
        public GameObject kunaiPickupPrefab;

        public Entity swordPickup;
        public Entity kunaiPickup;

        private Vector2 swordPosition;
        private Vector2 kunaiPosition;

        private void Awake()
        {
            swordPosition = swordPickup.transform.position;
            kunaiPosition = kunaiPickup.transform.position;
        }

        public override void OnInit(World world)
        {
            base.OnInit(world);

            var nekonin = world.GetSingleton("Nekonin");

            if (nekonin != null)
            {
                nekonin.attack.weaponData = null;
            }

            if (entity.world != null)
            {
                entity.world.onPickup += OnPickup;
            }
        }

        private void OnDestroy()
        {
            if (entity.world != null)
            {
                entity.world.onPickup -= OnPickup;
            }
        }

        private void OnPickup(Entity pickupEntity)
        {
            if (pickupEntity == swordPickup && kunaiPickup == null)
            {
                var kunaiObject = GameObject.Instantiate(kunaiPickupPrefab, kunaiPosition, 
                    Quaternion.identity, entity.transform);
                kunaiPickup = kunaiObject.GetComponent<Entity>();
            } else if (pickupEntity == kunaiPickup && swordPickup == null)
            {
                var swordObject = GameObject.Instantiate(swordPickupPrefab, swordPosition, 
                    Quaternion.identity, entity.transform);
                swordPickup = swordObject.GetComponent<Entity>();
            }

            if (pickupEntity.pickup.pickupType.Equals("weapon"))
            {
                // complete room after picking the weapon
                entity.room.state = RoomComponent.State.Completed;
                
                // if we get at least one weapon, then activate the exits
                var roomList = entity.world.entities.Where(e => e.roomExit != null).ToList();
                foreach (var e in roomList)
                {
                    e.roomExit.open = true;
                }
            }
        }
    }
}
