using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GBJAM7.Scripts
{
    [Serializable]
    public struct BuildOption
    {
        public int cost;
        public string name;
        public GameObject prefab;
    }

    [Serializable]
    public class PlayerData
    {
        public int resources;
        public List<BuildOption> buildOptions;
    }
    
    public class GameController : MonoBehaviour
    {
        public UnitSelector selector;

        public BoundsInt worldBounds;
        
        public BoundsInt cameraBounds;
        
        public Camera worldCamera;

        public UnitMovementArea movementArea;

        public UnitInfo unitInfo;

        public OptionsMenu playerActions;

        public OptionsMenu buildActions;
        
        public OptionsMenu unitActions;
        
        // TODO: scroll camera if moving outside world bounds

        public KeyCode leftKey;
        public KeyCode rigthKey;
        public KeyCode upKey;
        public KeyCode downKey;

        public KeyCode button1KeyCode;
        public KeyCode button2KeyCode;
        
        public KeyCode startKeyCode;
        public KeyCode selectKeyCode;

        public int currentPlayer;
        public List<PlayerData> players;
        
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
                        .FirstOrDefault(u => u.player == currentPlayer && Vector2.Distance(selector.transform.position, u.transform.position) < 0.5f);
                    
                    SelectUnit(unit);
                }
                else
                {
                    var enemyUnit = FindObjectsOfType<Unit>()
                        .FirstOrDefault(u => u.player != currentPlayer && Vector2.Distance(selector.transform.position, u.transform.position) < 0.5f);
                    
                    // cant select new unit while other selected for now...
                    
                    // if selected same unit, then show UI for actions
                    
                    // if selected another unit, show UI for actions
                    
                    // if selected terrain, then check for movement

                    // TODO: check range range
                    if (enemyUnit != null)
                    {
                        // show attack menu
                        
                        
                    } else
                    {
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
                    // if we are over a unit, then show unit's menu
                    // otherwise show general menu

                    if (selectorOverUnit != null && selectorOverUnit.player == currentPlayer &&
                        selectorOverUnit.currentActions > 0 && selectorOverUnit.unitType == Unit.UnitType.Unit)
                    {
                        selectedUnit = selectorOverUnit;
                        ShowUnitActions();
                    }
                    else
                    {
                        ShowPlayerActions();
                    }
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
            currentPlayer = (currentPlayer + 1) % players.Count;
            var player = players[currentPlayer];
            
            var playerUnits = FindObjectsOfType<Unit>().Where(u => u.player == currentPlayer).ToList();
            playerUnits.ForEach(u =>
            {
                u.currentMovements = u.totalMovements;
                u.currentActions = u.totalActions;
                player.resources += u.resources;
            });

//            playerUnits.ForEach(u =>
//            {
//                player.resources += u.resources;
//            });
        }

        private void ShowPlayerActions()
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
            waitingForAction = true;
        }

        private void ShowUnitActions()
        {
            unitActions.title = "";
            // TODO: show only possible actions given the unit location and other units
            // or show actions but disabled
            unitActions.Show(new List<Option>()
            {
                new Option {name = "Attack"},
                new Option {name = "Capture"},
                new Option {name = "Cancel"},
            }, OnUnitActionSelected, CancelMenuAction);
            waitingForAction = true;
        }
        
        public void SelectUnit(Unit unit)
        {
            var player = players[currentPlayer];
            
            if (unit == null)
                return;
            DeselectUnit();
            selectedUnit = unit;
            if (unit.unitType == Unit.UnitType.Unit)
            {
                if (unit.currentMovements == 0 || unit.currentActions == 0)
                {
                    DeselectUnit();
                    return;
                }
                
                if (unit.currentMovements > 0)
                {
                    movementArea.Show(unit);
                }
                else
                {
                    ShowUnitActions();
                }
                
            } else if (unit.unitType == Unit.UnitType.Spawner)
            {
                // only show unit actions if available
                if (unit.currentActions > 0)
                {
                    buildActions.title = $"Build {player.resources}";
                    buildActions.Show(player.buildOptions
                        .Select(o => new Option { name = $"{o.name} {o.cost}" }).ToList(), 
                        OnBuildOptionSelected, 
                        CancelMenuAction);
                    waitingForAction = true;
                }
                else
                {
                    // in the case of factories that cant move, we just avoid selecting them again if you can't build
                    // I suppose.
                    DeselectUnit();
                }
                
//                buildMenu.Show(unit);
            } else if (unit.unitType == Unit.UnitType.MainBase)
            {
                // we can't do anything with the main base
                DeselectUnit();
            }
        }

        private void OnUnitActionSelected(int optionIndex, Option option)
        {
            if (option.name.Equals("Attack"))
            {
                // TODO: show attack range + possible targets 
                // change game state to be waiting for target selection
                Debug.Log("Attack!");
                selectedUnit.currentActions--;
            }
            
            if (option.name.Equals("Capture"))
            {
                // TODO: show capture range
                // change game state to be waiting for target selection
                Debug.Log("Capture!!");
                selectedUnit.currentActions--;
            }
            
            if (option.name.Equals("Cancel"))
            {
                CancelMenuAction();
            }
            
            CompleteMenuAction();
        }

        private void OnBuildOptionSelected(int optionIndex, Option option)
        {
            var player = players[currentPlayer];
            
            // TODO: show spawn area, wait for selection, can be cancelled
            
            // for now, spawn new unit in same unit location
            var newUnitObject = 
                Instantiate(player.buildOptions[optionIndex].prefab, selectedUnit.transform.position, Quaternion.identity);
                
            var newUnit = newUnitObject.GetComponentInChildren<Unit>();
            newUnit.totalActions = 0;
            newUnit.totalMovements = 0;
            
            // consume money
            player.resources -= player.buildOptions[optionIndex].cost;
            
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
