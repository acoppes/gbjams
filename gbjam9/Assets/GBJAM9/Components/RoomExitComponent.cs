using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class RoomExitComponent : MonoBehaviour, IGameComponent
    {
        [NonSerialized]
        public bool mainUnitCollision;

        public float distance;

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, distance);
        }
    }
}