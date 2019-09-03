using UnityEngine;

namespace GBJAM7.Scripts
{
    public class UnitActionsArea : MonoBehaviour
    {
        public UnitMovementArea movementArea;
        public UnitMovementArea attackArea;

        public void Show(Unit unit, bool movement = true, bool attack = true)
        {
//            var distance = 0;
//            
//            distance += unit.currentMovements > 0 ? unit.movementDistance : 0;
//            distance += unit.currentActions > 0 ? unit.attackDistance : 0;

            if (movement)
            {
                movementArea.Show(unit.transform.position, 0, unit.movementDistance);
                if (attack)
                {
                    attackArea.Show(unit.transform.position, unit.movementDistance + 1, 
                        unit.movementDistance + unit.attackDistance);
                }
            } else if (attack)
            {
                attackArea.Show(unit.transform.position, 0, unit.attackDistance);
            }
        }
        
        public void Hide()
        {
            movementArea.Hide();
            attackArea.Hide();
        }
    }
}