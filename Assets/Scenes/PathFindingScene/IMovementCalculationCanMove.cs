using UnityEngine;

namespace Scenes.PathFindingScene
{
    public interface IMovementCalculationCanMove
    {
        bool CanMove(Vector2Int position);
    }
}