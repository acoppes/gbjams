using System;
using System.Collections.Generic;
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

        public OptionsMenu playerActions;

        public OptionsMenu buildActions;
        
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

        private bool waitingForAction;

//        public float movementRepeatDelay = 0.5f;
//        private float movementRepeatCooldown = 0.0f;
//
//        [NonSerialized]
//        public bool keyReady;

        [NonSerialized]
        public bool leftPressed;
        
        [NonSerialized]
        public bool rightPressed;

        [NonSerialized]
        public bool upPressed;

        [NonSerialized]
        public bool downPressed;
        
        [NonSerialized]
        public bool button1Pressed;
        
        [NonSerialized]
        public bool button2Pressed;
        
        private void Start()
        {
            playerActions.Hide();
            unitInfo.Hide();
            buildActions.Hide();
        }

        public void Update()
        {
            // TODO: controls state, like "if in selection mode, then allow movement"

            leftPressed = Input.GetKeyDown(leftKey);
            rightPressed = Input.GetKeyDown(rigthKey);
            
            upPressed = Input.GetKeyDown(upKey);
            downPressed = Input.GetKeyDown(downKey);
            
            var movement = new Vector2Int(0, 0);

            movement.x += leftPressed ? -1 : 0;
            movement.x += rightPressed ? 1 : 0;
            
            movement.y += upPressed ? 1 : 0;
            movement.y += downPressed ? -1 : 0;
            
            button1Pressed = Input.GetKeyDown(button1KeyCode);
            button2Pressed = Input.GetKeyDown(button2KeyCode);
            
//            movementRepeatCooldown -= Time.deltaTime;
//            
//            if (movementRepeatCooldown <= 0 && (movement.x != 0 || movement.y != 0))
//            {
//                movementRepeatCooldown = movementRepeatDelay;
//                keyReady = true;
//            }
//            else if (movement.x == 0 && movement.y == 0)
//            {
//                keyReady = true;
//                movementRepeatCooldown = 0;
//            }
            
            // if showing a any menu and waiting for action..
            if (waitingForAction)
            {
                // do stuff here
                
                // with up/down we move between actions

//                if (button1Pressed)
//                {
//                    // confirm selected action
//                    // for now we only have end turn...
//                    EndCurrentPlayerTurn();
//                    playerActions.Hide();
//                    showingMenu = false;
//                }
//
//                if (button2Pressed)
//                {
//                    playerActions.Hide();
//                    showingMenu = false;
//                }
                
                return;
            }
            
            var selectorOverUnit = FindObjectsOfType<Unit>()
                .FirstOrDefault(u => Vector2.Distance(selector.transform.position, u.transform.position) < 0.5f);

//            if (keyReady)
            selector.Move(movement);
            
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
                        .FirstOrDefault(u => Vector2.Distance(selector.transform.position, u.transform.position) < 0.5f);
                    SelectUnit(unit);
                }
                else
                {
                    // cant select new unit while other selected for now...
                    
                    // if selected same unit, then show UI for actions
                    
                    // if selected another unit, show UI for actions
                    
                    // if selected terrain, then check for movement

                    if (selectedUnit.currentMovements > 0)
                    {
                        var selectedUnitPosition = selectedUnit.transform.position / 1;
                        var selectorPosition = selector.transform.position / 1;

                        var distance = Mathf.RoundToInt(Mathf.Abs(selectedUnitPosition.x - selectorPosition.x) +
                                                        Mathf.Abs(selectedUnitPosition.y - selectorPosition.y));
                        if (distance <= selectedUnit.movementDistance)
                        {
                            selectedUnit.transform.position = selector.transform.position;
                            selectedUnit.currentMovements = 0;
                            DeselectUnit();
                        }
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
                    playerActions.Show(new List<Option>()
                    {
                        new Option()
                        {
                            name = "End turn"
                        },
                        new Option()
                        {
                            name = "Cancel"
                        }
                    }, OnPlayerActionsMenuOptionSelected, CancelMenuAction);
                    // playerActions.Show();
                    waitingForAction = true;
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

        private void OnPlayerActionsMenuOptionSelected(int optionIndex, Option option)
        {
            if (optionIndex == 0)
            {
                EndCurrentPlayerTurn();
                CompleteMenuAction();
            }
            else
            {
                CancelMenuAction();
            }
        }

        public void EndCurrentPlayerTurn()
        {
            FindObjectsOfType<Unit>().ToList().ForEach(u =>
            {
                u.currentMovements = u.totalMovements;
                u.currentActions = u.totalActions;
            });
        }

        public void SelectUnit(Unit unit)
        {
            if (unit == null)
                return;
            DeselectUnit();
            selectedUnit = unit;
            if (unit.unitType == Unit.UnitType.Unit)
            {
                if (unit.currentMovements > 0)
                {
                    movementArea.Show(unit);
                }
                else
                {
                    // we should show menu for unit actions at some point
                    DeselectUnit();
                }
            } else if (unit.unitType == Unit.UnitType.Spawner)
            {
                // only show unit actions if available
                if (unit.currentActions > 0)
                {
                    // TODO: get player actions from player?
                    buildActions.Show(new List<Option>()
                    {
                        new Option {name = "Ranger 20"},
                        new Option {name = "Sniper 50"},
                        new Option {name = "Guardian 90"},
                    }, OnBuildOptionSelected, CancelMenuAction);
                    waitingForAction = true;
                }
                else
                {
                    // in the case of factories that cant move, we just avoid selecting them again if you can't build
                    // I suppose.
                    DeselectUnit();
                }
                
//                buildMenu.Show(unit);
            }
        }

        private void OnBuildOptionSelected(int optionIndex, Option option)
        {
            if (optionIndex == 0)
            {
                // build ranger
                // consume money
            }
            
            if (optionIndex == 1)
            {
                // build ranger
            }
            
            if (optionIndex == 2)
            {
                // build ranger
            }

            selectedUnit.currentActions--;
            CompleteMenuAction();
        }

        public void DeselectUnit()
        {
            if (selectedUnit == null) 
                return;
            
            movementArea.Hide();
            // hide UI probably too here
            selectedUnit = null;
        }

        public void CancelMenuAction()
        {
            waitingForAction = false;
            DeselectUnit();
        }

        public void CompleteMenuAction()
        {
            waitingForAction = false;
            DeselectUnit();
        }

    }
}
