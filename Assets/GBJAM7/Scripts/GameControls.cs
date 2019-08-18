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

            while (Mathf.Abs(worldCamera.transform.position.x - selector.transform.position.x) > worldBounds.size.x)
            {
                var direction = selector.transform.position.x - worldCamera.transform.position.x;
                var d = direction / Mathf.Abs(direction);
                worldCamera.transform.position += new Vector3(d, 0,0);
            }
            
            while (Mathf.Abs(worldCamera.transform.position.y - selector.transform.position.y) > worldBounds.size.y)
            {
                var direction = selector.transform.position.y - worldCamera.transform.position.y;
                var d = direction / Mathf.Abs(direction);
                worldCamera.transform.position += new Vector3(0, d,0);
            }
            
            
            if (Input.GetKeyDown(button1KeyCode))
            {
                // search for unit in location
                if (selectedUnit == null)
                {
                    var unit = FindObjectsOfType<Unit>()
                        .FirstOrDefault(u => u.movementsLeft > 0 &&
                            Vector2.Distance(selector.transform.position, u.transform.position) < 0.5f);
                    SelectUnit(unit);
                }
                else
                {
                    // cant select new unit while other selected for now...
                    
                    // if selected same unit, then show UI for actions
                    
                    // if selected another unit, show UI for actions
                    
                    // if selected terrain, then check for movement

                    var selectedUnitPosition = selectedUnit.transform.position / 1;
                    var selectorPosition = selector.transform.position / 1;

                    var distance = Mathf.RoundToInt(Mathf.Abs(selectedUnitPosition.x - selectorPosition.x) +
                                                    Mathf.Abs(selectedUnitPosition.y - selectorPosition.y));
                    if (distance <= selectedUnit.movementDistance)
                    {
                        selectedUnit.transform.position = selector.transform.position;
                        selectedUnit.movementsLeft = 0;
                        DeselectUnit();
                    }
                    
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
