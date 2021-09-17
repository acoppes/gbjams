using System.Collections.Generic;
using UnityEngine;

namespace Scenes.PathFindingScene
{
    public class PathFinding
    {
        private IMovementCalculationCanMove canMove;

        public PathFinding(IMovementCalculationCanMove canMove)
        {
            this.canMove = canMove;
        }
        
        public MovementArea GetMovementArea(Vector2Int position, int distance)
        {
            var area = new MovementArea();

            var nodesToVisit = new List<MovementNode>();
            nodesToVisit.Add(new MovementNode(position, distance));

            var visitedNodes = new List<MovementNode>();
            
            while (nodesToVisit.Count > 0)
            {
                var node = nodesToVisit[0];
                visitedNodes.Add(node);
                
                nodesToVisit.Remove(node);

                // if node in range to position and valid, add to area
                if (node.distance < distance && !canMove.CanMove(node.position))
                {
                    continue;
                }

                area.nodes.Add(node);

                var neighbours = new List<MovementNode>()
                {
                    new MovementNode(node.position + new Vector2Int(1, 0), node.distance - 1),
                    new MovementNode(node.position + new Vector2Int(0, 1), node.distance - 1),
                    new MovementNode(node.position + new Vector2Int(-1, 0), node.distance - 1),
                    new MovementNode(node.position + new Vector2Int(0, -1), node.distance - 1)
                };
                
                neighbours.ForEach(n =>
                {
                    if (visitedNodes.Contains(n))
                        return;

                    if (n.distance < 0)
                        return; 
                    
                    nodesToVisit.Add(n);
                });
            }
            
            return area;
        }
    }
}