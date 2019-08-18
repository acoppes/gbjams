using System.Linq;
using UnityEngine;

namespace GBJAM7.Scripts
{
    public class GameControls : MonoBehaviour
    {
        public UnitSelector selector;

        public BoundsInt worldBounds;
        
        public Camera worldCamera;

        public UnitMovementArea movementArea;
        
        // TODO: scroll camera if moving outside world bounds

        public KeyCode leftKey;
        public KeyCode rigthKey;
        public KeyCode upKey;
        public KeyCode downKey;

        public KeyCode button1KeyCode;
        public KeyCode button2KeyCode;
        
//        private enum State
//        {
//            None,
//            UnitSelected
//        }
//
//        private State state;

        private Unit selectedUnit;
        
        public void Update()
        {
            // TODO: controls state, like "if in selection mode, then allow movement"
            
            if (Input.GetKeyDown(leftKey))
            {
                selector.Move(new Vector2Int(-1, 0));
            }
            
            if (Input.GetKeyDown(rigthKey))
            {
                selector.Move(new Vector2Int(1, 0));
            }
            
            if (Input.GetKeyDown(upKey))
            {
                selector.Move(new Vector2Int(0, 1));
            }
            
            if (Input.GetKeyDown(downKey))
            {
                selector.Move(new Vector2Int(0, -1));
            }
            
            if (Input.GetKeyDown(button1KeyCode))
            {
                // search for unit in location
                if (selectedUnit == null)
                {
                    var unit = FindObjectsOfType<Unit>()
                        .FirstOrDefault(u => 
                            Vector2.Distance(selector.transform.position, u.transform.position) < 2.0f);
                    SelectUnit(unit);
                }
                else
                {
                    // cant select new unit while other selected for now...
                    
                    // if selected same unit, then show UI for actions
                    
                    // if selected another unit, show UI for actions
                    
                    // if selected terrain, then check for movement
                    
                    // here we wait for movement and confirmation

//                    if (IsValidMovement())
//                    {
//                        MoveUnit();
//                        ConsumeUnitMovement();
//                        deselect the unit
//                    }
                }

            }

            if (Input.GetKeyDown(button2KeyCode))
            {
                DeselectUnit();
            }
        }

        public void SelectUnit(Unit unit)
        {
            if (unit == null)
                return;
            DeselectUnit();
            selectedUnit = unit;
            movementArea.Show(unit);
        }

        public void DeselectUnit()
        {
            if (selectedUnit == null) 
                return;
            
            movementArea.Hide();
            // hide UI probably too here
            selectedUnit = null;
        }
    }
}
