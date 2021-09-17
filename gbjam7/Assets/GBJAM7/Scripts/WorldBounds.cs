using System;
using UnityEngine;

namespace GBJAM7.Scripts
{
    public class WorldBounds : MonoBehaviour
    {
        public BoundsInt bounds;

        public BoundsInt GetBounds()
        {
            var b = bounds;
            b.position = Vector3Int.RoundToInt(transform.position);
            return b;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, bounds.size * 2);
        }
    }
}
