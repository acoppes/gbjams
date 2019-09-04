using System;
using System.Collections.Generic;
using System.Linq;
using GBJAM7.Scripts;
using GBJAM7.Scripts.MainMenu;
using UnityEngine;

namespace Scenes.PathFindingScene
{
    // PathFinding class
    // getMovementArea(p0, d) : movementArea
    
    // Movement area class
    // m.GetAttackArea(d) : node[]
    // m.CanMove(p1) : bool
    
    public class PathFindingSceneController : MonoBehaviour, IMovementCalculationCanMove
    {
        public Unit unit;
        public int distance;

        public UnitMovementArea unitMovementArea;

        public UnitSelector unitSelector;

        private List<MovementObstacleBase> cachedMovementObstacles;

        [SerializeField]
        private GameboyButtonKeyMapAsset keyAsset;

        private MovementArea movementArea;

        private void RecalculateMovementArea()
        {
            var p0 = Vector2Int.RoundToInt(unit.transform.position);
            cachedMovementObstacles = FindObjectsOfType<MovementObstacleBase>().ToList();
            movementArea = new PathFinding(this).GetMovementArea(p0, distance);
        }

        private void Start()
        {
            RecalculateMovementArea();
            unitMovementArea.Hide();
            unitMovementArea.Show(movementArea.nodes.Select(n=> n.position).ToList());
        }

        // Update is called once per frame
        private void Update()
        {
            keyAsset.UpdateControlState();
            
            var movement = new Vector2Int(0, 0);

            movement.x += keyAsset.leftPressed ? -1 : 0;
            movement.x += keyAsset.rightPressed ? 1 : 0;
            
            movement.y += keyAsset.upPressed ? 1 : 0;
            movement.y += keyAsset.downPressed ? -1 : 0;
            
            unitSelector.Move(movement);
            
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {

                // show that path in mov area
            }

            if (keyAsset.button1Pressed && movementArea != null)
            {
                if (movementArea.CanMove(Vector2Int.RoundToInt(unitSelector.position)))
                {
                    unit.transform.position = unitSelector.position;
                    // recalculate
                    RecalculateMovementArea();
                    unitMovementArea.Hide();
                    unitMovementArea.Show(movementArea.nodes.Select(n=> n.position).ToList());
                }
            }
        }

        public bool CanMove(Vector2Int position)
        {
            var obstaclesCount = cachedMovementObstacles.Count(o => o.IsBlocked(position));

            if (obstaclesCount == 0)
                return true;
            
            return false;
        }
    }
}
