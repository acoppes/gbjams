using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GBJAM7.Scripts.MainMenu;
using Scenes.PathFindingScene;
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
    
    public class GameController : MonoBehaviour, IMovementCalculationCanMove
    {
        public UnitSelector selector;
        
        public BoundsInt cameraBounds;

        private WorldBounds worldBounds;
        
        public GameCamera gameCamera;

        public UnitActionsArea unitActionsArea;
        public UnitActionsArea unitActionsPreviewArea;       
       
        public GameInfo gameInfo;
        public UnitInfo unitInfo;

        public GameHud gameHud;
        public ChangeTurnSequence changeTurnSequence;
        public AttackSequence attackSequence;
        public GameOverController gameOverController;
        
        public OptionsMenu playerActions;
        public OptionsMenu buildActions;
        public OptionsMenu unitActions;
        public OptionsMenu generalOptionsMenu;
        
        // TODO: scroll camera if moving outside world bounds

        public GameboyButtonKeyMapAsset keyMapAsset;

        [SerializeField]
        private AudioSource _unitDeploySfx;
        
        [SerializeField]
        private AudioSource _invalidActionSfx;
        
        [SerializeField]
        private AudioSource _unitAttackStructureSfx;
        
        public int currentTurn;
        
        public GameObject unitDeathPrefab;
        
        public int currentPlayer;
        public List<PlayerData> players;

        private Unit selectedUnit;

        private bool waitingForMenuAction;

        private bool waitingForAttackTarget;

        private bool waitingForMovement;
        
        const float minHealthToDestroy = 0.01f;
        
        private bool showingAttackSequence;
        private bool showingChangeTurnSequence;

        private PathFinding pathFinding;
        private MovementArea movementArea;

        private void Start()
        {
            worldBounds = FindObjectOfType<WorldBounds>();
            
            playerActions.Hide();
            unitInfo.Hide();
            buildActions.Hide();
            
            pathFinding = new PathFinding(this);
            
            var startLocation = GameObject.Find("~StartLocation");

            if (startLocation != null)
            {
                StartShowChangeTurnUI(startLocation.transform.position);
            }

        }

        public void Update()
        {
            // TODO: controls state, like "if in selection mode, then allow movement"
            
            Utils.UpdateEnemiesInRange();
            UpdateObstacles();

            keyMapAsset.UpdateControlState();
            
            var movement = new Vector2Int(0, 0);

            movement.x += keyMapAsset.leftPressed ? -1 : 0;
            movement.x += keyMapAsset.rightPressed ? 1 : 0;
            
            movement.y += keyMapAsset.upPressed ? 1 : 0;
            movement.y += keyMapAsset.downPressed ? -1 : 0;
            
            gameInfo.UpdateGameInfo(currentPlayer, currentTurn);
            
            if (showingAttackSequence)
            {
                if (keyMapAsset.button2Pressed)
                {
                    attackSequence.ForceComplete();
                }
                return;
            }
            
            if (showingChangeTurnSequence)
            {
                if (keyMapAsset.button2Pressed)
                {
                    changeTurnSequence.ForceComplete();
                }
                return;
            }

            // if showing a any menu and waiting for action..
            var selectorOverUnit = FindObjectsOfType<Unit>()
                .FirstOrDefault(u => Vector2.Distance(selector.position, u.transform.position) < 0.5f);

            if (waitingForMenuAction)
            {
                if (keyMapAsset.button2Pressed)
                {
                    buildActions.Hide();
                    playerActions.Hide();
                    unitActions.Hide();
                    waitingForMenuAction = false;
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
                        Utils.IsInDistance(source.transform.position, target.transform.position, 
                            source.attackDistance))
                    {
                        // do damage to target
                        // do damage back from target to unit
                        var distance = Utils.GetDistance(source.transform.position, target.transform.position);
      
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
//                        else
//                        {
//                            attackSequenceData.counterAttack = false;
//                        }

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
                        
                        unitActionsArea.Hide();
                    
                        DeselectUnit();

                        StartCoroutine(routine: StartAttackSequence(attackSequenceData, source, target));
                    }
                    else
                    {
                        if (_invalidActionSfx != null)
                        {
                            _invalidActionSfx.Play();
                        }
                    }

                }
                
                // is button 2 pressed, cancel
                if (keyMapAsset.button2Pressed)
                {
                    // go back to unit actions menu
//                    attackArea.Hide();
                    unitActionsArea.Hide();

                    DeselectUnit();
//                    ShowUnitActions();
                    waitingForAttackTarget = false;
                    
//                    DeselectUnit();
//                    waitingForAttackTarget = false;

                    // show menu again?
                }

                return;
            }

            if (waitingForMovement)
            {
                selector.Move(movement);
                AdjustCameraToSelector();

                if (keyMapAsset.button1Pressed)
                {
                    // can't move over our structures
                    if (selectedUnit.currentMovements > 0 && selectorOverUnit == null)
                    {
                        var p0 = selectedUnit.transform.position / 1;
                        var p1 = selector.position / 1;

//                        var obstacle = obstacles.FirstOrDefault(o => o.IsBlocked(Vector2Int.RoundToInt(selector.position)));

                        if (movementArea.CanMove(Vector2Int.RoundToInt(p1))) 
                        {
//                        if (Utils.IsInDistance(p0, p1, selectedUnit.movementDistance) && obstacle == null)
//                        {
                            selectedUnit.transform.position = selector.position;
                            selectedUnit.currentMovements = 0;

                            selectedUnit.moveDirection = p1 - p0;

                            waitingForMovement = false;

                            if (selectedUnit.currentActions > 0)
                            {
                                unitActionsArea.Hide();
                                StartWaitingForAttackTarget();
                            } else
                            {
                                DeselectUnit();
                            }
                        }
                        else
                        {
                            if (_invalidActionSfx != null)
                            {
                                _invalidActionSfx.Play();
                            }
                        }
                        
                    } else if (selectorOverUnit == selectedUnit)
                    {
                        unitActionsArea.Hide();
                        waitingForMovement = false;
                        ShowUnitActions();
                    }
                    else
                    {
                        if (_invalidActionSfx != null)
                        {
                            _invalidActionSfx.Play();
                        }
                    }
                }

                // is button 2 pressed, cancel
                if (keyMapAsset.button2Pressed)
                {
                    unitActionsArea.Hide();
//                        movementArea.Hide();
//                        attackArea.Hide();
                    DeselectUnit();
                    waitingForMovement = false;
                }

                return;
            }

            
            // if waiting for any action { 
            
            selector.Move(movement);
            AdjustCameraToSelector();

            if (keyMapAsset.button1Pressed)
            {
                // search for unit in location
                if (selectedUnit == null)
                {
                    var unit = FindObjectsOfType<Unit>()
                        .FirstOrDefault(u => u.player == currentPlayer && Vector2.Distance(selector.position, u.transform.position) < 0.5f);

                    if (unit == null)
                    {
                        if (selectorOverUnit == null)
                        {
                            ShowPlayerActions();
                        }
                        else
                        {
                            // play error selection sound
                        }
                    }
                    else
                    {
                        SelectUnit(unit);    
                    }
                }
            }

            if (keyMapAsset.button2Pressed)
            {
                var playerUnits = FindObjectsOfType<Unit>().Where(u =>
                {
                    if (u.player != currentPlayer)
                        return false;
                    if (u.currentMovements == 0 && u.currentActions == 0)
                        return false;

                    // if unit and can only attack and no enemies in range..
                    if (u.currentMovements == 0 && u.unitType == Unit.UnitType.Unit)
                    {
                        return u.enemiesInRange > 0;
                    }
                    
                    return true;
                }).ToList();

                var index = 0;
                
                if (selectorOverUnit != null)
                {
                    index = playerUnits.IndexOf(selectorOverUnit);
                    index++;
                }

                if (playerUnits.Count > 0)
                {
                    // TODO: consider only units with possible actions, so if no targets in range
                    // for attack, then don't consider that unit. Or maybe just consider units with movement or 
                    // spawners with action.
                    
                    index %= playerUnits.Count;
                    // index = Mathf.Clamp(index, 0, playerUnits.Count - 1);
                    var unit = playerUnits[index];
                    selector.position = unit.transform.position;
                    AdjustCameraToSelector();
                }
            }

            if (selectorOverUnit != null && selectedUnit == null)
            {
                unitActionsPreviewArea.Hide();
//                previewArea.Hide();
                
                unitInfo.Preview(currentPlayer, selectorOverUnit);


                
                if (selectorOverUnit.player == currentPlayer &&
                    (selectorOverUnit.currentMovements > 0 || selectorOverUnit.currentActions > 0))
                {
                    movementArea = pathFinding.GetMovementArea(Vector2Int.RoundToInt(selectorOverUnit.transform.position), 
                        selectorOverUnit.currentMovements > 0 ? selectorOverUnit.movementDistance : 0);
                    
                    if (selectorOverUnit.currentMovements > 0)
                        unitActionsPreviewArea.ShowMovement(movementArea.GetPositions());
                    
                    if (selectorOverUnit.currentActions > 0)
                        unitActionsPreviewArea.ShowAttack(movementArea.GetExtraNodes(selectorOverUnit.attackDistance));
                    
//                    unitActionsPreviewArea.Show(selectorOverUnit, selectorOverUnit.currentMovements > 0, 
//                        selectorOverUnit.currentActions > 0);
                } else if (selectorOverUnit.player != currentPlayer)
                {
                    movementArea = pathFinding.GetMovementArea(Vector2Int.RoundToInt(selectorOverUnit.transform.position), 
                        selectorOverUnit.movementDistance);
                    
                    unitActionsPreviewArea.ShowMovement(movementArea.GetPositions());
                    unitActionsPreviewArea.ShowAttack(movementArea.GetExtraNodes(selectorOverUnit.attackDistance));
//                    unitActionsPreviewArea.Show(selectorOverUnit);
                }
                
            }
            else
            {
                unitInfo.Hide();
                unitActionsPreviewArea.Hide();
//                previewArea.Hide();
            }

            if (keyMapAsset.startPressed)
            {
                gameHud.Hide();
                waitingForMenuAction = true;
                
                // show options menu and wait for options
                generalOptionsMenu.Show(new List<Option>()
                {
                    new Option { name = "Continue" },
//                    new Option { name = "Restart" },
                    new Option { name = "Main Menu" },
                }, OnGeneralMenuOptionSelected, OnGeneralMenuCanceled);
            }

        }

        private void OnGeneralMenuCanceled()
        {
            generalOptionsMenu.Hide();
            gameHud.Show();
            waitingForMenuAction = false;
        }

        private bool OnGeneralMenuOptionSelected(int i, Option option)
        {
            if ("Continue".Equals(option.name))
            {
                generalOptionsMenu.Hide();
                gameHud.Show();
                waitingForMenuAction = false;
            }
            
//            if ("Restart".Equals(option.name))
//            {
//                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//            }
            
            if ("Main Menu".Equals(option.name))
            {
                SceneManager.LoadScene("MainMenuScene");
            }

            return true;
        }

        private IEnumerator StartAttackSequence(AttackSequenceData attackSequenceData, Unit source, Unit target)
        {
            // hide menues!!

            // dont show attack sequence if attacking a structure
            if (source.attackSequenceUnitPrefab != null && target.attackSequenceUnitPrefab != null 
                                                        && target.unitType == Unit.UnitType.Unit)
            {
                attackSequenceData.player1Data = players[0];
                attackSequenceData.player2Data = players[1];
                    
                showingAttackSequence = true;
                
                var localPosition = attackSequence.transform.localPosition;
                attackSequence.transform.localPosition = new Vector3(0, 0, localPosition.z);

                attackSequence.Show(attackSequenceData);

                yield return new WaitUntil(() => attackSequence.completed);

                attackSequence.transform.localPosition = localPosition;

                showingAttackSequence = false;
                
                // TODO: center camera in unit position
            }
            
            if (source.currentHP <= minHealthToDestroy)
            {
                GameObject.Instantiate(unitDeathPrefab, source.transform.position, Quaternion.identity);
                Destroy(source.gameObject);
            }
            
            if (target.unitType != Unit.UnitType.Unit)
            {
                if (_unitAttackStructureSfx != null)
                {
                    _unitAttackStructureSfx.Play();
                }
            }

            if (target.currentHP <= minHealthToDestroy)
            {
                if (target.unitType == Unit.UnitType.Unit)
                {
                    GameObject.Instantiate(unitDeathPrefab, target.transform.position, Quaternion.identity);
                    Destroy(target.gameObject);
                }
                else
                {
                    // can't capture if unit dies during capture (if counter attack)
                    // or if unit cant capture or if too far away
                    if (source == null || !source.canCapture)
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


            // Check victory condition...
            CheckVictoryCondition();
            
        }

        private void CheckVictoryCondition()
        {
            
            // if hero is dead or no more refineries, then victory for some player
            
            // check first current player and then next player
            
            var player1Defeated = IsPlayerDefeated(0);
            var player2Defeated = IsPlayerDefeated(1);

            if (!player1Defeated && !player2Defeated)
                return;
            
            gameOverController.StartSequence(this, new GameOverData
            {
                defeatedPlayer = player1Defeated ? 0 : 1,
                player1 = players[0],
                player2 = players[1]
            });
        }

        private bool IsPlayerDefeated(int player)
        {
            var playerDefeated = false;
            var units = FindObjectsOfType<Unit>().Where(u => u.player == player);

            playerDefeated = playerDefeated || units.Count(u => u.isHero && u.currentHP >= minHealthToDestroy) == 0;
            playerDefeated = playerDefeated ||
                                    units.Count(u =>
                                        u.unitType == Unit.UnitType.MainBase && u.currentHP >= minHealthToDestroy) == 0;

            return playerDefeated;
        }

        private void AdjustCameraToSelector()
        {
            var t = gameCamera;
            
            if (worldBounds != null)
            {
                // do adjust back
                var bounds = worldBounds.GetBounds();

                var p = Vector3Int.RoundToInt(selector.position);

                p.x = Mathf.Clamp(p.x, -bounds.xMax + 1, bounds.xMax);
                p.y = Mathf.Clamp(p.y, -bounds.yMax + 1, bounds.yMax);

                selector.position = p;
            }
            
            while (Mathf.Abs(t.position.x - selector.position.x) > cameraBounds.size.x)
            {
                var direction = selector.position.x - t.position.x;
                var d = direction / Mathf.Abs(direction);
                t.position += new Vector3(d, 0,0);
            }
            
            while (Mathf.Abs(t.position.y - selector.position.y) > cameraBounds.size.y)
            {
                var direction = selector.position.y - t.position.y;
                var d = direction / Mathf.Abs(direction);
                t.position += new Vector3(0, d,0);
            }

        }

        private bool OnPlayerActionSelected(int optionIndex, Option option)
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

            return true;
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

            var centerPosition = selector.position;
            
            var heroUnit = FindObjectsOfType<Unit>().FirstOrDefault(u => u.player == currentPlayer 
                                                                         && u.unitType == Unit.UnitType.Unit && u.isHero);
            if (heroUnit != null)
            {
                centerPosition = heroUnit.transform.position;
            }

            StartShowChangeTurnUI(centerPosition);
        }

        public void StartShowChangeTurnUI(Vector2 centerPosition)
        {
            StartCoroutine(ShowChangeTurnUI(centerPosition));
        }

        private IEnumerator ShowChangeTurnUI(Vector2 centerPosition)
        {
            // just wait one frame
//            yield return null;
            
            gameHud.Hide();
            
            // center new turn in specified position
            selector.position = centerPosition;
            gameCamera.position = centerPosition;
            
            changeTurnSequence.Show(players[currentPlayer], currentPlayer, currentTurn);
            showingChangeTurnSequence = true;
            
            // Hide all menues
            // block game input
            // set the change turn ui and show it
            // once completed, turn back everything
            yield return new WaitUntil(() => changeTurnSequence.completed);

            gameHud.Show();
            showingChangeTurnSequence = false;
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
            waitingForMenuAction = true;
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
            waitingForMenuAction = true;
        }
        
        public void SelectUnit(Unit unit)
        {
            var player = players[currentPlayer];
            
            if (unit == null)
                return;
            DeselectUnit();
            selectedUnit = unit;

            movementArea = pathFinding.GetMovementArea(Vector2Int.RoundToInt(selectedUnit.transform.position),
                unit.currentMovements > 0 ? unit.movementDistance : 0);
            
            if (unit.unitType == Unit.UnitType.Unit)
            {
                if (unit.currentMovements == 0 && unit.currentActions == 0)
                {
                    DeselectUnit();
                    return;
                }
                
                if (unit.currentMovements > 0)
                {
                    StartWaitingForMovement();
                }
                else
                {
                    StartWaitingForAttackTarget();
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
                    waitingForMenuAction = true;
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
        
        private void StartWaitingForMovement()
        {
            waitingForMenuAction = false;
            waitingForMovement = true;
            unitActions.Hide();
            
            unitActionsArea.ShowMovement(movementArea.GetPositions());
            unitActionsArea.ShowAttack(movementArea.GetExtraNodes(selectedUnit.attackDistance));
        }

        private void StartWaitingForAttackTarget()
        {
            waitingForMenuAction = false;
            waitingForAttackTarget = true;
            unitActions.Hide();
            
            movementArea = pathFinding.GetMovementArea(Vector2Int.RoundToInt(selectedUnit.transform.position), 
                0);
            
            unitActionsArea.ShowAttack(movementArea.GetExtraNodes(selectedUnit.attackDistance));
        }

        private bool OnUnitActionSelected(int optionIndex, Option option)
        {
            if (option.name.Equals("Attack"))
            {
                StartWaitingForAttackTarget();
                return true;
            }

            if (option.name.Equals("Cancel"))
            {
                CancelMenuAction();
            }
            
            CompleteMenuAction();
            
            unitActions.Hide();

            return false;
        }

        private bool OnBuildOptionSelected(int optionIndex, Option option)
        {
            var player = players[currentPlayer];
            
            // TODO: show spawn area, wait for selection, can be cancelled
            
            // for now, spawn new unit in same unit location
            var buildOption = player.buildOptions[optionIndex];

            if (player.resources < buildOption.unitPrefab.cost)
            {
                return false;
            }
            
            var newUnitObject = 
                Instantiate(buildOption.unitPrefab.gameObject, selectedUnit.transform.position, Quaternion.identity);

            if (_unitDeploySfx != null)
            {
                _unitDeploySfx.Play();
            }
                
            var newUnit = newUnitObject.GetComponentInChildren<Unit>();
            newUnit.currentActions = 0;
            newUnit.currentMovements = 0;
            newUnit.player = currentPlayer;
            
            // consume money
            player.resources -= buildOption.unitPrefab.cost;
            
            selectedUnit.currentActions--;
            CompleteMenuAction();
            
            buildActions.Hide();

            return true;
        }

        public void DeselectUnit()
        {
            if (selectedUnit == null) 
                return;
            
            unitActionsArea.Hide();
//            movementArea.Hide();
//            attackArea.Hide();
            // hide UI probably too here
            selectedUnit = null;
        }

        public void CancelMenuAction()
        {
            waitingForMenuAction = false;
            DeselectUnit();
        }

        public void CompleteMenuAction()
        {
            waitingForMenuAction = false;
            DeselectUnit();
        }

        public void BlockPlayerActions()
        {
            waitingForMenuAction = true;
        }

        public void HideMenus()
        {
            gameHud.Hide();
        }
        
        private List<MovementObstacleBase> obstacles = new List<MovementObstacleBase>();

        private void UpdateObstacles()
        {
            obstacles = FindObjectsOfType<MovementObstacleBase>().ToList();    
        }
        
        public bool CanMove(Vector2Int position)
        {
            var obstaclesCount = obstacles.Count(o => o.IsBlocked(position));

            if (obstaclesCount == 0)
                return true;
            
            return false;
        }
    }
}
