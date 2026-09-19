using System.Collections.Generic;
using System.Linq;
using Game.Components;
using GBJAM14.Components;
using GBJAM14.GamePlay;
using GBJAM14.UI;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Utilities;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyBox;
using UnityEngine;

namespace GBJAM14.Systems
{
    public class InteractionSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        // private readonly EcsFilterInject<Inc<InteractableComponent>, Exc<DisabledComponent>> 
        //     interactables = default;
        
        private readonly EcsFilterInject<Inc<InteractableFocusedByPlayerComponent>, Exc<DisabledComponent>> 
            focusables = default;
        
        private readonly EcsFilterInject<Inc<CharacterComponent, InteractableComponent, InteractableFocusedByPlayerComponent>, Exc<DisabledComponent>> 
            focusableCharacters = default;
        
        private readonly EcsFilterInject<Inc<ModelInstanceComponent>, Exc<DisabledComponent, InteractableFocusedByPlayerComponent>> 
            interactableModels = default;
        
        private readonly EcsFilterInject<Inc<InteractableComponent, ModelInstanceComponent, InteractableFocusedByPlayerComponent>, Exc<DisabledComponent>> 
            interactableModelsFocused = default;
        
        private readonly EcsFilterInject<Inc<InteractActionComponent>, Exc<DisabledComponent>> 
            interactActions = default;
        
        private readonly EcsFilterInject<Inc<DialogComponent, DestroyableComponent>, Exc<DisabledComponent>> 
            dialogs = default;
        
        private CharacterDialogsDB characterDialogsDB;
        private GameUIManager gameUIManager;
        
        public void Init(EcsSystems systems)
        {
            characterDialogsDB = FindFirstObjectByType<CharacterDialogsDB>();
            gameUIManager = FindFirstObjectByType<GameUIManager>();
        }

        private void ShowDialogOptions(Entity source, Entity target, List<CharacterDialogsDB.DialogData> options)
        {
            var characterA = source.Get<CharacterComponent>();
            var characterB = target.Get<CharacterComponent>();

            var optionDataList = options.Select(d => new Option()
            {
                disabled = false,
                name = d.option,
                userData = d, 
                callback = option =>
                {
                    var optionDialogData = option.userData as CharacterDialogsDB.DialogData;
                                    
                    var dialogEntity = world.CreateEntity();
                    dialogEntity.Add(new DialogComponent()
                    {
                        source = source,
                        target = target,
                        characterId = characterB.characterId,
                        dialogId = optionDialogData.id,
                        dialogData = optionDialogData,
                        completed = false,
                        showOptionsOnComplete = false
                    });
                    dialogEntity.Add(new DestroyableComponent());
                        
                    gameUIManager.uiDialog.ShowDialog(optionDialogData, new List<string>()
                    {
                        characterA.characterId,
                        characterB.characterId
                    }, dialogEntity);
                }
            }).ToList();
            
            optionDataList.Add(new Option()
            {
                disabled = false,
                name = "Leave", 
                callback = _ =>
                {
                    gameUIManager.dialogOptions.window.Close();        
                }
            });
            
            gameUIManager.dialogOptions.ShowOptions(optionDataList);
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in interactActions.Value)
            {
                var interactAction = interactActions.Pools.Inc1.Get(e);

                if (interactAction.target.Has<CharacterComponent>())
                {
                    ref var inventory = ref interactAction.source.Get<InventoryComponent>();
                    var characterA = interactAction.source.Get<CharacterComponent>();

                    var characterB = interactAction.target.Get<CharacterComponent>();
                
                    var dialogs = characterDialogsDB.GetCharacterDialogs(characterB.characterId, 
                        inventory.items);

                    if (!string.IsNullOrEmpty(interactAction.optionalDialogId))
                    {
                        var dialog = characterDialogsDB.GetDialog(interactAction.optionalDialogId);
                        dialogs = new List<CharacterDialogsDB.DialogData>()
                        {
                            dialog
                        };
                    }

                    if (dialogs.Count > 0)
                    {
                        var options = dialogs.Where(d => !string.IsNullOrEmpty(d.option)).ToList();

                        if (options.Count > 0)
                        {
                            ShowDialogOptions(interactAction.source, interactAction.target, options);
                        }
                        else
                        {
                            var dialogData = dialogs.GetRandom();
                        
                            var dialogEntity = world.CreateEntity();
                            dialogEntity.Add(new DialogComponent()
                            {
                                source = interactAction.source,
                                target = interactAction.target,
                                characterId = characterB.characterId,
                                dialogId = dialogData.id,
                                dialogData = dialogData,
                                completed = false,
                                showOptionsOnComplete = true
                            });
                            dialogEntity.Add(new DestroyableComponent());
                        
                            gameUIManager.uiDialog.ShowDialog(dialogData, new List<string>()
                            {
                                characterA.characterId,
                                characterB.characterId
                            }, dialogEntity);
                        }
                    }

                    if (interactAction.target.Has<ItemComponent>())
                    {
                        interactAction.target.Get<DestroyableComponent>().destroy = true;
                    }
                    
                    if (interactAction.target.Has<InteractableFocusedByPlayerComponent>())
                    {
                        interactAction.target.Remove<InteractableFocusedByPlayerComponent>();
                        // interactAction.target.Get<InteractableComponent>().focusedByPlayer = false;
                    }
                }

                interactActions.Pools.Inc1.Del(e);
            }

            var dialogCompleted = false;
            
            foreach (var e in dialogs.Value)
            {
                ref var dialog = ref dialogs.Pools.Inc1.Get(e);
                if (!dialog.completed)
                {
                    if (gameUIManager.uiDialog.window.IsClosed())
                    {
                        var dialogData = dialog.dialogData;
                        ref var inventory = ref dialog.source.Get<InventoryComponent>();
                        
                        if (dialogData.output != null)
                        {
                            foreach (var statusId in dialogData.output)
                            {
                                if (statusId.StartsWith("+"))
                                {
                                    inventory.items.Add(statusId.Substring(1));
                                } else if (statusId.StartsWith("-"))
                                {
                                    inventory.items.Remove(statusId.Substring(1));
                                }
                            }
                        }

                        if (dialog.showOptionsOnComplete)
                        {
                            var dialogs = characterDialogsDB.GetCharacterDialogs(dialog.characterId, 
                                inventory.items);
                            var options = dialogs.Where(d => !string.IsNullOrEmpty(d.option)).ToList();
                         
                            if (options.Count > 0)
                            {
                                ShowDialogOptions(dialog.source, dialog.target, options);
                            }
                        }

                        
                        // save game!! 
                        dialogCompleted = true;
                        dialog.completed = true;
                        dialogs.Pools.Inc2.Get(e).destroy = true;
                    }
                }
            }

            if (dialogCompleted)
            {
                ActionUtils.SaveGameSave(world);
            }
            
            foreach (var e in interactableModels.Value)
            {
                // var interactable = interactableModels.Pools.Inc1.Get(e);
                var model = interactableModels.Pools.Inc1.Get(e);
                var interactObject = model.modelGameObject.transform.FindInHierarchy("InteractHighlight");
                if (interactObject)
                {
                    interactObject.gameObject.SetActive(false);
                    // interactObject.gameObject.SetActive(interactable.focusedByPlayer && interactable.showInteractBubble);
                }
            }
            
            foreach (var e in interactableModelsFocused.Value)
            {
                var interactable = interactableModelsFocused.Pools.Inc1.Get(e);
                var model = interactableModelsFocused.Pools.Inc2.Get(e);
                var interactObject = model.modelGameObject.transform.FindInHierarchy("InteractHighlight");
                if (interactObject)
                {
                    interactObject.gameObject.SetActive(interactable.showInteractBubble);
                    interactObject.localPosition = new Vector3(0, interactable.bubbleOffset, 0);
                    // interactObject.gameObject.SetActive(interactable.focusedByPlayer && interactable.showInteractBubble);
                }
            }
            
            string focusedCharacter = null;
            
            foreach (var e in focusableCharacters.Value)
            {
                var character = focusableCharacters.Pools.Inc1.Get(e);
                var interactable = focusableCharacters.Pools.Inc2.Get(e);
                
                if (interactable.showFocusedUI)
                {
                    focusedCharacter = character.characterId;
                }
            }

            if (gameUIManager)
            {
                if (string.IsNullOrEmpty(focusedCharacter))
                {
                    gameUIManager.uiFocusedCharacter.Hide();
                }
                else
                {
                    gameUIManager.uiFocusedCharacter.Show(focusedCharacter);
                }
            }
            
            foreach (var e in focusables.Value)
            {
                focusables.Pools.Inc1.Del(e);
            }
        }
    }
}