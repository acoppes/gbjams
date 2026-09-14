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
                        entity.Get<RoomNavigationComponent>().currentExit = this;
                        if (SignalsManager.instance)
                        {
                            SignalsManager.instance.onExitRoomSignal.Signal(entityReference.entity);
                        }
                    }
                }
            }
        }
    }
}