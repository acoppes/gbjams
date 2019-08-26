using System;
using UnityEngine;

namespace Scenes.PathFindingScene
{
    [ExecuteInEditMode]
    public class MovementObstacle : MovementObstacleBase
    {
        [NonSerialized]
        public Vector2Int position;

        private void Start()
        {
            position = Vector2Int.RoundToInt(transform.position);
        }

        private void LateUpdate()
        {
            position = Vector2Int.RoundToInt(transform.position);
        }

        public override bool IsBlocked(Vector2Int position)
        {
            return this.position.Equals(position);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));
        }
    }
}