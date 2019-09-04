using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scenes.PathFindingScene
{
    public class MovementArea
    {
        public List<MovementNode> nodes = new List<MovementNode>();

        public bool CanMove(Vector2Int p)
        {
            return nodes.Count(n => n.position == p) > 0;
        }
    }
}