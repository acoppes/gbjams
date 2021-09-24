using System;
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

        private void OnPickup(Entity pickupEntity)
        {
            Debug.Log("ON PICKUP");
            if (pickupEntity == swordPickup && kunaiPickup == null)
            {
                var kunaiObject = GameObject.Instantiate(kunaiPickupPrefab, kunaiPosition, Quaternion.identity);
                kunaiPickup = kunaiObject.GetComponent<Entity>();
            } else if (pickupEntity == kunaiPickup && swordPickup == null)
            {
                var swordObject = GameObject.Instantiate(swordPickupPrefab, swordPosition, Quaternion.identity);
                swordPickup = swordObject.GetComponent<Entity>();
            }
        }
    }
}
