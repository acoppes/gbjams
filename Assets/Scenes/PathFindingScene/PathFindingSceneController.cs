using System.Collections.Generic;
using System.Linq;
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

    public class MovementArea
    {
        public List<MovementNode> nodes = new List<MovementNode>(); 
    }

    public interface MovementCalculationCanMove
    {
        bool CanMove(Vector2Int position);
    }

    public class MovementCalculation
    {
        private MovementCalculationCanMove canMove;

        public MovementCalculation(MovementCalculationCanMove canMove)
        {
            this.canMove = canMove;
        }
        
        public MovementArea GetMovementNodes(Vector2Int position, int distance)
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
    
    public class PathFindingSceneController : MonoBehaviour, MovementCalculationCanMove
    {
        public Transform testTransform;
        public int distance;

        public UnitMovementArea unitMovementArea;

        private List<MovementObstacle> cachedMovementObstacles;
        
        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                unitMovementArea.Hide();
                
                // calculate movement area for one point
                var position = Vector2Int.RoundToInt(testTransform.position);
                
                cachedMovementObstacles = FindObjectsOfType<MovementObstacle>().ToList();
                
                var movementArea = new MovementCalculation(this).GetMovementNodes(position, distance);
                
                unitMovementArea.Show(movementArea.nodes.Select(n=> n.position).ToList());
                // show that path in mov area
            }
        }


        public bool CanMove(Vector2Int position)
        {
            var obstaclesCount = cachedMovementObstacles.Count(o => o.position.Equals(position));

            if (obstaclesCount == 0)
                return true;
            
            return false;
        }
    }
}
