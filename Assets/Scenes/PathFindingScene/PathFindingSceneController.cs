using System.Collections.Generic;
using System.Linq;
using GBJAM7.Scripts;
using UnityEngine;

namespace Scenes.PathFindingScene
{
    public class MovementNode
    {
        public Vector2Int position;

        public MovementNode(Vector2Int position)
        {
            this.position = position;
        }

        public int GetDistance(Vector2Int position)
        {
            return Utils.GetDistance(this.position, position);
        }

        public override bool Equals(object obj)
        {
            if (obj is MovementNode node)
            {
                return node.position == position;
            }
            return base.Equals(obj);
        }
    }

    public class MovementArea
    {
        public List<MovementNode> nodes = new List<MovementNode>(); 
    }

    public class MovementCalculation
    {
        public bool IsValidForMovement(Vector2Int position)
        {
            return true;
        }
        
        public MovementArea GetMovementNodes(Vector2Int position, int distance)
        {
            var area = new MovementArea();

            var nodesToVisit = new List<MovementNode>();
            nodesToVisit.Add(new MovementNode(position));

            var visitedNodes = new List<MovementNode>();
            
            while (nodesToVisit.Count > 0 && distance > 0)
            {
                var node = nodesToVisit[0];
                visitedNodes.Add(node);
                nodesToVisit.Remove(node);

                // if node in range to position and valid, add to area
                if (IsValidForMovement(node.position) && node.GetDistance(position) <= distance)
                {
                    area.nodes.Add(node);
                }

                var neighbours = new List<MovementNode>()
                {
                    new MovementNode(node.position + new Vector2Int(1, 0)),
                    new MovementNode(node.position + new Vector2Int(0, 1)),
                    new MovementNode(node.position + new Vector2Int(-1, 0)),
                    new MovementNode(node.position + new Vector2Int(0, -1))
                };
                
                neighbours.ForEach(n =>
                {
                    if (!visitedNodes.Contains(n)) 
                        nodesToVisit.Add(n);
                });
                
                distance--;
            }
            
            return area;
        }
    }
    
    public class PathFindingSceneController : MonoBehaviour
    {
        public Transform testTransform;
        public int distance;

        public UnitMovementArea unitMovementArea;
        
        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                unitMovementArea.Hide();
                
                // calculate movement area for one point
                var position = Vector2Int.RoundToInt(testTransform.position);
                var movementArea = new MovementCalculation().GetMovementNodes(position, distance);
                
                unitMovementArea.Show(movementArea.nodes.Select(n=> n.position).ToList());
                // show that path in mov area
            }
        }
    }
}
