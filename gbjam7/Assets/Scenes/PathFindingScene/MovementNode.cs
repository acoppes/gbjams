using GBJAM7.Scripts;
using UnityEngine;

namespace Scenes.PathFindingScene
{
    public class MovementNode
    {
        public int distance;
        public Vector2Int position;

        public MovementNode(Vector2Int position, int distance)
        {
            this.position = position;
            this.distance = distance;
        }

        public int GetDistance(Vector2Int position)
        {
            return Utils.GetDistance(this.position, position);
        }

        public override bool Equals(object obj)
        {
            if (obj is MovementNode node)
            {
                return node.position.Equals(position);
            }
            return base.Equals(obj);
        }
    }
}