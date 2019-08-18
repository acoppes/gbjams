using System;
using System.Linq;
using UnityEngine;

namespace GBJAM7.Scripts
{
    public class GameControls : MonoBehaviour
    {
        public UnitSelector selector;

        public BoundsInt worldBounds;
        
        public BoundsInt cameraBounds;
        
        public Camera worldCamera;

        public UnitMovementArea movementArea;

        public UnitInfo unitInfo;

        public PlayerActions playerActions;
        
        // TODO: scroll camera if moving outside world bounds

        public KeyCode leftKey;
        public KeyCode rigthKey;
        public KeyCode upKey;
        public KeyCode downKey;

        public KeyCode button1KeyCode;
        public KeyCode button2KeyCode;
        
        public KeyCode startKeyCode;
        public KeyCode selectKeyCode;
        
//        private enum State
//        {
//            None,
//            UnitSelected
//        }
//
//        private State state;

        private Unit selectedUnit;

        private bool showingPlayerActions;

        public float movementRepeatDelay = 0.5f;
        private float movementRepeatCooldown = 0.0f;

        private void Start()
        {
            playerActions.Hide();
            unitInfo.Hide();
        }

        public void Update()
        {
            // TODO: controls state, like "if in selection mode, then allow movement"

            var leftPressed = Input.GetKey(leftKey);
            var rightPressed = Input.GetKey(rigthKey);
            
            var upPressed = Input.GetKey(upKey);
            var downPressed = Input.GetKey(downKey);
            
            var movement = new Vector2Int(0, 0);

            movement.x += leftPressed ? -1 : 0;
            movement.x += rightPressed ? 1 : 0;
            
            movement.y += upPressed ? 1 : 0;
            movement.y += downPressed ? -1 : 0;
            
            var button1Pressed = Input.GetKeyDown(button1KeyCode);
            var button2Pressed = Input.GetKeyDown(button2KeyCode);
            
            if (showingPlayerActions)
            {
                // do stuff here
                
                // with up/down we move between actions

                if (button1Pressed)
                {
                    // confirm selected action
                    // for now we only have end turn...
                    EndCurrentPlayerTurn();
                    playerActions.Hide();
                    showingPlayerActions = false;
                }

                if (button2Pressed)
                {
                    playerActions.Hide();
                    showingPlayerActions = false;
                }
                
                return;
            }
            
            var selectorOverUnit = FindObjectsOfType<Unit>()
                .FirstOrDefault(u => Vector2.Distance(selector.transform.position, u.transform.position) < 0.5f);

            movementRepeatCooldown -= Time.deltaTime;
            
            if (movementRepeatCooldown <= 0 && (movement.x != 0 || movement.y != 0))
            {
                selector.Move(movement);
                movementRepeatCooldown = movementRepeatDelay;
            }
            else if (movement.x == 0 && movement.y == 0)
            {
                movementRepeatCooldown = 0;
            }
            
            // if not in world limits already then pan the camera
            while (Mathf.Abs(worldCamera.transform.position.x - selector.transform.position.x) > cameraBounds.size.x)
            {
                var direction = selector.transform.position.x - worldCamera.transform.position.x;
                var d = direction / Mathf.Abs(direction);
                worldCamera.transform.position += new Vector3(d, 0,0);
            }
            
            while (Mathf.Abs(worldCamera.transform.position.y - selector.transform.position.y) > cameraBounds.size.y)
            {
                var direction = selector.transform.position.y - worldCamera.transform.position.y;
                var d = direction / Mathf.Abs(direction);
                worldCamera.transform.position += new Vector3(0, d,0);
            }
            
            
            if (button1Pressed)
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

            if (button2Pressed)
            {
                if (selectedUnit != null)
                {
                    DeselectUnit();
                }
                else
                {
                    playerActions.Show();
                    showingPlayerActions = true;
                }
            }

            if (selectorOverUnit != null && selectedUnit == null)
            {
                unitInfo.Preview(selectorOverUnit);
            }
            else
            {
                unitInfo.Hide();
            }
            
        }

        private void EndCurrentPlayerTurn()
        {
            FindObjectsOfType<Unit>().ToList().ForEach(u => u.movementsLeft = 1);
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
