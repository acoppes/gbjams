using UnityEngine;

namespace Scenes.PathFindingScene
{
    public abstract class MovementObstacleBase : MonoBehaviour
    {
        public abstract bool IsBlocked(Vector2Int position);
    }
}