using System;
using UnityEngine;

namespace GBJAM10.Components
{
    public class RoomExitComponent : MonoBehaviour, IEntityComponent
    {
        [NonSerialized]
        public bool playerInExit;

        public float distance;

        [NonSerialized]
        public bool open;

        [NonSerialized]
        public string rewardType;

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, distance);
        }
    }
}