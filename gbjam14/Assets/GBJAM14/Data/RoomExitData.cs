using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace GBJAM14.Data
{
    public class RoomExitData : MonoBehaviour
    {
        public BoxCollider2D exitCollider;

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.attachedRigidbody)
            {
                var entityReference = other.attachedRigidbody.GetComponent<EntityReference>();
                if (entityReference)
                {
                    if (SignalsManager.instance)
                    {
                        SignalsManager.instance.onExitRoomSignal.Signal(entityReference.entity);
                    }
                }
            }
        }
    }
}