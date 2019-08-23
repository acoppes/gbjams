using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GBJAM7.Scripts.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GBJAM7.Scripts
{
    [Serializable]
    public struct BuildOption
    {
        public string name;
        public Unit unitPrefab;
    }

    [Serializable]
    public class PlayerData
    {
        public string name;
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
        public UnitMovementArea attackArea;
//        public UnitMovementArea captureArea;

        public GameInfo gameInfo;
        public UnitInfo unitInfo;

        public GameHud gameHud;
        public ChangeTurnSequence changeTurnSequence;
        public AttackSequence attackSequence;
        
        public OptionsMenu playerActions;
        public OptionsMenu buildActions;
        public OptionsMenu unitActions;
        public OptionsMenu generalOptionsMenu;
        
        // TODO: scroll camera if moving outside world bounds

        public GameboyButtonKeyMapAsset keyMapAsset;

        public int currentTurn;
        
        public int currentPlayer;
        public List<PlayerData> players;
        
        private Unit selectedUnit;

        private bool waitingForAction;

        private bool waitingForAttackTarget;
//        private bool waitingForCaptureTarget;

        private bool showingAttackSequence;

//        public float movementRepeatDelay = 0.5f;
//        private float movementRepeatCooldown = 0.0f;
//
//        [NonSerialized]
//        public bool keyReady;

        private void Start()
        {
            playerActions.Hide();
            unitInfo.Hide();
            buildActions.Hide();
        }

        public void Update()
        {
            // TODO: controls state, like "if in selection mode, then allow movement"

            keyMapAsset.UpdateControlState();
            
            var movement = new Vector2Int(0, 0);

            movement.x += keyMapAsset.leftPressed ? -1 : 0;
            movement.x += keyMapAsset.rightPressed ? 1 : 0;
            
            movement.y += keyMapAsset.upPressed ? 1 : 0;
            movement.y += keyMapAsset.downPressed ? -1 : 0;
            
            gameInfo.UpdateGameInfo(currentPlayer, currentTurn);
            
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

            if (showingAttackSequence)
            {
                if (keyMapAsset.button2Pressed)
                {
                    attackSequence.ForceComplete();
                }
                return;
            }

            // if showing a any menu and waiting for action..
            var selectorOverUnit = FindObjectsOfType<Unit>()
                .FirstOrDefault(u => Vector2.Distance(selector.transform.position, u.transform.position) < 0.5f);
            
            if (waitingForAction)
            {
                if (keyMapAsset.button2Pressed)
                {
                    buildActions.Hide();
                    playerActions.Hide();
                    unitActions.Hide();
                    waitingForAction = false;
                    DeselectUnit();
                }
                
                return;
            }

            if (waitingForAttackTarget)
            {
                // we want to allow moving the selector while targeting a unit
                selector.Move(movement);
                AdjustCameraToSelector();
                
                if (keyMapAsset.button1Pressed)
                {
                    // if inside attack area and there is an enemy there, attack enemy

                    var target = selectorOverUnit;
                    var source = selectedUnit;

                  
                    if (target != null && target.player != currentPlayer &&
                        IsInDistance(source.transform.position, target.transform.position, 
                            source.attackDistance))
                    {
                        // do damage to target
                        // do damage back from target to unit
                        var distance = GetDistance(source.transform.position, target.transform.position);
      
                        var attackSequenceData = new AttackSequenceData()
                        {
                            player1UnitPrefab = source.attackSequenceUnitPrefab,
                            player2UnitPrefab = target.attackSequenceUnitPrefab,
                            playerAttacking = currentPlayer,
                            counterAttack = distance <= target.attackDistance,
                            player1Units = Mathf.CeilToInt(source.squadSize * source.currentHP / source.totalHP),
                            player2Units = Mathf.CeilToInt(target.squadSize * target.currentHP / target.totalHP),
                        };

                        var sourceDmg = source.dmg * (source.currentHP / source.totalHP);
                        
                        target.currentHP -= sourceDmg;
                        Debug.Log($"{target.name} received {sourceDmg} dmg");
                        if (target.currentHP > 0)
                        {
                            var targetDmg = target.dmg * (target.currentHP / target.totalHP);
                            source.currentHP -= targetDmg;
                            Debug.Log($"{source.name} received {targetDmg} dmg");
                        }

                        var p1CurrentUnits = Mathf.CeilToInt(source.squadSize * source.currentHP / source.totalHP);
                        var p2CurrentUnits = Mathf.CeilToInt(target.squadSize * target.currentHP / target.totalHP);
                        
                        attackSequenceData.player1Killed = attackSequenceData.player1Units - p1CurrentUnits;
                        attackSequenceData.player2Killed = attackSequenceData.player2Units - p2CurrentUnits;
                        
                        // show attack sequence...
                    
                        // TODO: show attack range + possible targets 
                        // change game state to be waiting for target selection
                        Debug.Log("Attack!");
                        
                        // consume attack

                        source.currentActions--;
                        // we consume movement after attack too
                        if (source.currentMovements > 0)
                            source.currentMovements--;
                    
                        waitingForAttackTarget = false;
                        attackArea.Hide();
                    
                        DeselectUnit();

                        StartCoroutine(routine: StartAttackSequence(attackSequenceData, source, target));
                    }

                }
                
                // is button 2 pressed, cancel
                if (keyMapAsset.button2Pressed)
                {
                    // go back to unit actions menu
                    attackArea.Hide();
                    DeselectUnit();
//                    ShowUnitActions();
                    waitingForAttackTarget = false;
                    
//                    DeselectUnit();
//                    waitingForAttackTarget = false;

                    // show menu again?
                }

                return;
            }
            
            selector.Move(movement);
            AdjustCameraToSelector();
            
            if (keyMapAsset.button1Pressed)
            {
                // search for unit in location
                if (selectedUnit == null)
                {
                    var unit = FindObjectsOfType<Unit>()
                        .FirstOrDefault(u => u.player == currentPlayer && Vector2.Distance(selector.transform.position, u.transform.position) < 0.5f);

                    if (unit == null)
                    {
                        ShowPlayerActions();
                    }
                    else
                    {
                        SelectUnit(unit);    
                    }
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
                        // if enemy unit inside range (movement + attack range)
                        // show attack menu, and if clicked, then move to nearest position and attack...
                    } else
                    {
                        // can't move over our structures
                        if (selectedUnit.currentMovements > 0 && selectorOverUnit == null)
                        {
                            var p0 = selectedUnit.transform.position / 1;
                            var p1 = selector.transform.position / 1;
                            
                            if (IsInDistance(p0, p1, selectedUnit.movementDistance))
                            {
                                selectedUnit.transform.position = selector.transform.position;
                                selectedUnit.currentMovements = 0;

                                selectedUnit.moveDirection = p1 - p0;

                                if (selectedUnit.currentActions > 0)
                                {
                                    movementArea.Hide();
                                    attackArea.Hide();
                                    
                                    StartWaitingForAttackTarget();
                                    
//                                    movementArea.Show(selectedUnit.transform.position, selectedUnit.actionDistance);
//                                    ShowUnitActions();
                                } else
                                {
                                    DeselectUnit();
                                }
                            }
                        } else if (selectorOverUnit == selectedUnit)
                        {
                            movementArea.Hide();
                            attackArea.Hide();
                            ShowUnitActions();
                        }
                        
//                        if (selectorOverUnit == selectedUnit)
//                        {
//                            ShowUnitActions();
//                        }
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

            if (keyMapAsset.button2Pressed)
            {
                var playerUnits = FindObjectsOfType<Unit>().Where(u =>
                    u.player == currentPlayer && (u.currentMovements > 0 || u.currentActions > 0)).ToList();

                var index = 0;
                
                if (selectorOverUnit != null)
                {
                    index = playerUnits.IndexOf(selectorOverUnit);
                    index++;
                }

                if (playerUnits.Count > 0)
                {
                    index %= playerUnits.Count;
                    // index = Mathf.Clamp(index, 0, playerUnits.Count - 1);
                    var unit = playerUnits[index];
                    selector.transform.position = unit.transform.position;
                    AdjustCameraToSelector();
//                    SelectUnit(unit);
                }
                
//                if (selectedUnit != null)
//                {
//                    DeselectUnit();
//                }
//                else
//                {
//                    // if we are over a unit, then show unit's menu
//                    // otherwise show general menu
//
////                    if (selectorOverUnit != null && selectorOverUnit.player == currentPlayer &&
////                        selectorOverUnit.currentActions > 0 && selectorOverUnit.unitType == Unit.UnitType.Unit)
////                    {
////                        selectedUnit = selectorOverUnit;
////                        ShowUnitActions();
////                    }
////                    else
////                    {
////                        
////                    }
//                    
//                    ShowPlayerActions();
//                }
            }

            if (selectorOverUnit != null && selectedUnit == null)
            {
                unitInfo.Preview(currentPlayer, selectorOverUnit);
            }
            else
            {
                unitInfo.Hide();
            }

            if (keyMapAsset.startPressed)
            {
                gameHud.Hide();
                waitingForAction = true;
                
                // show options menu and wait for options
                generalOptionsMenu.Show(new List<Option>()
                {
                    new Option { name = "Continue" },
                    new Option { name = "Restart" },
                    new Option { name = "Main Menu" },
                }, OnGeneralMenuOptionSelected, OnGeneralMenuCanceled);
            }
            
        }

        private void OnGeneralMenuCanceled()
        {
            generalOptionsMenu.Hide();
            gameHud.Show();
            waitingForAction = false;
        }

        private void OnGeneralMenuOptionSelected(int i, Option option)
        {
            if ("Continue".Equals(option.name))
            {
                generalOptionsMenu.Hide();
                gameHud.Show();
                waitingForAction = false;
            }
            
            if ("Restart".Equals(option.name))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            
            if ("Main Menu".Equals(option.name))
            {
                SceneManager.LoadScene("MainMenuScene");
            }
        }

        private IEnumerator StartAttackSequence(AttackSequenceData attackSequenceData, Unit source, Unit target)
        {
            // hide menues!!

            // dont show attack sequence if attacking a structure
            if (source.attackSequenceUnitPrefab != null && target.attackSequenceUnitPrefab != null 
                                                        && target.unitType == Unit.UnitType.Unit)
            {
                attackSequenceData.player1Data = players[source.player];
                attackSequenceData.player2Data = players[target.player];
                    
                showingAttackSequence = true;
                
                var localPosition = attackSequence.transform.localPosition;
                attackSequence.transform.localPosition = new Vector3(0, 0, localPosition.z);

                attackSequence.Show(attackSequenceData);

                yield return new WaitUntil(() => attackSequence.completed);

                attackSequence.transform.localPosition = localPosition;

                showingAttackSequence = false;
                
                // TODO: center camera in unit position
            }

            if (Mathf.RoundToInt(source.currentHP) <= 0)
            {
                // TODO: show explosions for units killed
                Destroy(source.gameObject);
            }
            
            if (Mathf.RoundToInt(target.currentHP) <= 0)
            {
                if (target.unitType == Unit.UnitType.Unit)
                {
                    // TODO: show explosions for units killed
                    Destroy(target.gameObject);
                }
                else
                {
                    // can't capture if unit dies during capture (if counter attack)
                    // or if unit cant capture or if too far away
                    if (source == null || !source.canCapture || GetDistance(source, target) > 1)
                    {
                        target.player = -1;
                        // leave it in 10% so you have to attack it a bit to capture it again next time
                        target.currentHP = Mathf.CeilToInt(target.totalHP * 0.05f);
                        Debug.Log($"{target.name} lost capture but couldn't be captured by distance or unit can't capture.");
                    }
                    else
                    {
                        target.player = source.player;
                        var percentage = source.currentHP * 0.5f / source.totalHP;
                        target.currentHP = Mathf.CeilToInt(target.totalHP * percentage);
                        Debug.Log($"{target.name} captured by player {source.player} with {percentage * 100}% health");
                        
                        // captured structures don't have action in this turn
                        target.currentActions = 0;
                        target.currentMovements = 0;
                    }
                }
            }
        }

        private void AdjustCameraToSelector()
        {
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
        }
        
        public int GetDistance(Unit a, Unit b)
        {
            return GetDistance(a.transform.position, b.transform.position);
        }

        public int GetDistance(Vector2 a, Vector2 b)
        {
            return Mathf.RoundToInt(Mathf.Abs(a.x - b.x) +
                                            Mathf.Abs(a.y - b.y));
        }

        public bool IsInDistance(Vector2 a, Vector2 b, int distance)
        {
            return GetDistance(a, b) <= distance;
        }

        private void OnPlayerActionSelected(int optionIndex, Option option)
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
            
            playerActions.Hide();
        }

        public void EndCurrentPlayerTurn()
        {
            currentPlayer = (currentPlayer + 1) % players.Count;
            
            var playerUnits = FindObjectsOfType<Unit>().Where(u => u.player == currentPlayer).ToList();
            playerUnits.ForEach(u =>
            {
                u.currentMovements = u.totalMovements;
                u.currentActions = u.totalActions;
            });

            if (currentPlayer == 0)
            {
                currentTurn++;
                // regen money for everyone
                
                FindObjectsOfType<Unit>().ToList().ForEach(u =>
                {
                    if (u.player >= 0)
                        players[u.player].resources += u.resources;
                    
                    if (u.regenHP > 0)
                    {
                        u.currentHP = Mathf.Min(u.totalHP, u.currentHP + u.regenHP);
                    }
                });
                
            }

            StartCoroutine(ShowChangeTurnUI());
        }

        private IEnumerator ShowChangeTurnUI()
        {
            gameHud.Hide();
            
            // tween camera
            var heroUnit = FindObjectsOfType<Unit>().FirstOrDefault(u => u.player == currentPlayer 
                                                                         && u.unitType == Unit.UnitType.Unit && u.isHero);
            if (heroUnit != null)
            {
                selector.transform.position = heroUnit.transform.position;
                AdjustCameraToSelector();
                // TODO: tween camera to selector!!
            }
            
            changeTurnSequence.Show(players[currentPlayer], currentPlayer, currentTurn);
            waitingForAction = true;
            
            // Hide all menues
            // block game input
            // set the change turn ui and show it
            // once completed, turn back everything
            yield return new WaitUntil(() => changeTurnSequence.completed);
            


            gameHud.Show();
            waitingForAction = false;
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
            }, OnPlayerActionSelected, CancelMenuAction);
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
//                new Option {name = "Capture"},
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
                if (unit.currentMovements == 0 && unit.currentActions == 0)
                {
                    DeselectUnit();
                    return;
                }
                
                if (unit.currentMovements > 0)
                {
                    movementArea.Show(unit.transform.position, 0, unit.movementDistance);
                    attackArea.Show(unit.transform.position, unit.movementDistance + 1, unit.movementDistance + unit.attackDistance);
                }
                else
                {
                    StartWaitingForAttackTarget();
//                    ShowUnitActions();
                }
                
            } else if (unit.unitType == Unit.UnitType.Spawner)
            {
                // only show unit actions if available
                if (unit.currentActions > 0)
                {
                    buildActions.title = $"Build {player.resources}";
                    buildActions.Show(player.buildOptions
                        .Select(o => new Option { name = $"{o.name} {o.unitPrefab.cost}" }).ToList(), 
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

        private void StartWaitingForAttackTarget()
        {
            waitingForAction = false;
            waitingForAttackTarget = true;
            unitActions.Hide();
            attackArea.Show(selectedUnit.transform.position, 0, selectedUnit.attackDistance);
        }

        private void OnUnitActionSelected(int optionIndex, Option option)
        {
            if (option.name.Equals("Attack"))
            {
                StartWaitingForAttackTarget();
                return;
            }

            if (option.name.Equals("Cancel"))
            {
                CancelMenuAction();
            }
            
            CompleteMenuAction();
            
            unitActions.Hide();
        }

        private void OnBuildOptionSelected(int optionIndex, Option option)
        {
            var player = players[currentPlayer];
            
            // TODO: show spawn area, wait for selection, can be cancelled
            
            // for now, spawn new unit in same unit location
            var buildOption = player.buildOptions[optionIndex];

            if (player.resources < buildOption.unitPrefab.cost)
                return;
            
            var newUnitObject = 
                Instantiate(buildOption.unitPrefab.gameObject, selectedUnit.transform.position, Quaternion.identity);
                
            var newUnit = newUnitObject.GetComponentInChildren<Unit>();
            newUnit.currentActions = 0;
            newUnit.currentMovements = 0;
            newUnit.player = currentPlayer;
            
            // consume money
            player.resources -= buildOption.unitPrefab.cost;
            
            selectedUnit.currentActions--;
            CompleteMenuAction();
            
            buildActions.Hide();
        }

        public void DeselectUnit()
        {
            if (selectedUnit == null) 
                return;
            
            movementArea.Hide();
            attackArea.Hide();
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
