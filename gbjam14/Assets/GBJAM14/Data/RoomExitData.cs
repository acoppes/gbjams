using System;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace GBJAM14.Data
{
    public class RoomExitData : MonoBehaviour
    {
        public RoomStartData roomStartData;
        
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
                        entity.Get<RoomNavigationComponent>().nextRoomId = roomStartData.GetComponentInParent<RoomData>().name;
                        entity.Get<RoomNavigationComponent>().nextStartId = roomStartData.name;

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
            if (roomStartData)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, roomStartData.transform.position);
                Gizmos.DrawSphere(transform.position, 0.25f);
            }
        }
    }
}