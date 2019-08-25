using System;
using UnityEngine;

namespace Scenes.PathFindingScene
{
    [ExecuteInEditMode]
    public class MovementObstacle : MonoBehaviour
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));
        }
    }
}