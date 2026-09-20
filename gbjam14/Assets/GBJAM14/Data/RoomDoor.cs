using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace GBJAM14.Data
{
    public class RoomDoor : MonoBehaviour
    {
        public Vector3 EnterPosition => transform.position + enterPosition;

        public Vector2 EnterDirection => enterPosition.normalized;

        [SerializeField] 
        private Vector3 enterPosition;
        
        // public Transform enter;
        
        public RoomDoor nextRoomDoor;
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.attachedRigidbody)
            {
                var entityReference = other.attachedRigidbody.GetComponent<EntityReference>();
                if (entityReference)
                {
                    var entity = entityReference.entity;
                    if (entity.Has<RoomNavigationComponent>())
                    {
                        entity.Get<RoomNavigationComponent>().nextRoomId = nextRoomDoor.GetComponentInParent<Room>().name;
                        entity.Get<RoomNavigationComponent>().nextEnterId = nextRoomDoor.name;

                        if (SignalsManager.instance)
                        {
                            SignalsManager.instance.onExitRoomSignal.Signal(entityReference.entity);
                        }
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            // if (nextRoomDoor && nextRoomDoor.enter)
            // {
            //     Gizmos.color = Color.blue;
            //     Gizmos.DrawLine(transform.position, nextRoomDoor.enter.transform.position);
            //     Gizmos.DrawSphere(transform.position, 0.25f);
            // }
            
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.25f);
            
            if (nextRoomDoor)
            {
                Gizmos.DrawLine(transform.position, nextRoomDoor.EnterPosition);
            }
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(EnterPosition, 0.15f);
        }
    }
}