using System.Collections.Generic;
using UnityEngine;

namespace GBJAM7.Scripts
{
    public class UnitActionsArea : MonoBehaviour
    {
        public UnitMovementArea movementArea;
        public UnitMovementArea attackArea;

        public void ShowMovement(List<Vector2Int> movementNodes)
        {
            movementArea.Show(movementNodes);
        }
        
        public void ShowAttack(List<Vector2Int> attackNodes)
        {
            attackArea.Show(attackNodes);
        }

//        public void Show(Unit unit, bool movement = true, bool attack = true)
//        {
//            if (movement)
//            {
//                movementArea.Show(unit.transform.position, 0, unit.movementDistance);
//                if (attack)
//                {
//                    attackArea.Show(unit.transform.position, unit.movementDistance + 1, 
//                        unit.movementDistance + unit.attackDistance);
//                }
//            } else if (attack)
//            {
//                attackArea.Show(unit.transform.position, 0, unit.attackDistance);
//            }
//        }
        
        public void Hide()
        {
            movementArea.Hide();
            attackArea.Hide();
        }
    }
}