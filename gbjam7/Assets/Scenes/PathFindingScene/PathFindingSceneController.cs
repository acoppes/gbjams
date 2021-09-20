using System;
using System.Collections.Generic;
using System.Linq;
using GBJAM.Commons;
using GBJAM7.Scripts;
using GBJAM7.Scripts.MainMenu;
using UnityEngine;
using UnityEngine.Serialization;

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
        
        [FormerlySerializedAs("distance")] public int movementDistance;
        public int attackDistance = 2;
        
        public UnitMovementArea unitMovementArea;
        public UnitMovementArea attackArea;
        
        public UnitSelector unitSelector;

        private List<MovementObstacleBase> cachedMovementObstacles;

        [SerializeField]
        private GameboyButtonKeyMapAsset keyAsset;

        private MovementArea movementArea;
        
        private void RecalculateMovementArea()
        {
            var p0 = Vector2Int.RoundToInt(unit.transform.position);
            cachedMovementObstacles = FindObjectsOfType<MovementObstacleBase>().ToList();
            movementArea = new PathFinding(this).GetMovementArea(p0, movementDistance);
        }

        private void Start()
        {
            RecalculateMovementArea();
            unitMovementArea.Hide();
            attackArea.Hide();
            unitMovementArea.Show(movementArea.GetPositions());
            attackArea.Show(movementArea.GetExtraNodes(attackDistance));
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

            if (keyAsset.button1Pressed && movementArea != null)
            {
                if (movementArea.CanMove(Vector2Int.RoundToInt(unitSelector.position)))
                {
                    unit.transform.position = unitSelector.position;
                    // recalculate
                    RecalculateMovementArea();
                    unitMovementArea.Hide();
                    attackArea.Hide();
                    unitMovementArea.Show(movementArea.GetPositions());
                    attackArea.Show(movementArea.GetExtraNodes(attackDistance));
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
