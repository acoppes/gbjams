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

        public List<Vector2Int> GetPositions()
        {
            return nodes.Select(n => n.position).ToList();
        }

        public List<Vector2Int> GetExtraNodes(int minDistance, int maxDistance)
        {
            var attackNodes = new HashSet<MovementNode>();
            
            nodes.ForEach(n =>
            {
                // expand n in distance
                if (minDistance == 0 && maxDistance == 0)
                    return;
            
                var p = new Vector2Int(-maxDistance, -maxDistance);
                for (var i = p.x; i <= maxDistance; i++)
                {
                    for (var j = p.y; j <= maxDistance; j++)
                    {
                        var totalDistance = Mathf.Abs(i) + Mathf.Abs(j);
                        if (totalDistance <= maxDistance && totalDistance >= minDistance)
                        {
                            var newPosition = n.position + new Vector2Int(i, j);
                            attackNodes.Add(new MovementNode(newPosition, totalDistance));
                        }
                    }
                }
            });

            attackNodes.RemoveWhere(n => nodes.Contains(n));
            return attackNodes.Select(n => n.position).ToList();
        }
    }
}