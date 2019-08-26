using UnityEngine;

namespace Scenes.PathFindingScene
{
    public class PolygonColliderObstacle : MovementObstacleBase
    {
        public PolygonCollider2D polygonCollider;

        public bool walkable;
        
        public override bool IsBlocked(Vector2Int position)
        {
            var overlapped = polygonCollider.OverlapPoint(position);
            return walkable ? !overlapped : overlapped;
        }
    }
}