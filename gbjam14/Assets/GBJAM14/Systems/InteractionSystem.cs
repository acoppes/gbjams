using System.Collections.Generic;
using System.Linq;
using Game.Components;
using GBJAM14.Components;
using GBJAM14.UI;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Utilities;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyBox;

namespace GBJAM14.Systems
{
    public class InteractionSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<InteractableComponent>, Exc<DisabledComponent>> 
            interactables = default;
        
        private readonly EcsFilterInject<Inc<InteractableComponent, ModelInstanceComponent>, Exc<DisabledComponent>> 
            interactableModels = default;
        
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

                    if (dialogs.Count > 0)
                    {
                        var options = dialogs.Where(d => !string.IsNullOrEmpty(d.option)).ToList();

                        if (options.Count > 0)
                        {
                            
                            gameUIManager.dialogOptions.ShowOptions(dialogs.Select(d => new Option()
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
                                        sourceEntity = interactAction.source,
                                        characterId = characterB.characterId,
                                        dialogId = optionDialogData.id,
                                        dialogData = optionDialogData,
                                        completed = false
                                    });
                                    dialogEntity.Add(new DestroyableComponent());
                        
                                    gameUIManager.uiDialog.ShowDialog(optionDialogData, new List<string>()
                                    {
                                        characterA.characterId,
                                        characterB.characterId
                                    }, dialogEntity);
                                }
                            }).ToList());
                        }
                        else
                        {
                            var dialogData = dialogs.GetRandom();
                        
                            var dialogEntity = world.CreateEntity();
                            dialogEntity.Add(new DialogComponent()
                            {
                                sourceEntity = interactAction.source,
                                characterId = characterB.characterId,
                                dialogId = dialogData.id,
                                dialogData = dialogData,
                                completed = false
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
                    
                    interactAction.target.Get<InteractableComponent>().focusedByPlayer = false;
                }

                interactActions.Pools.Inc1.Del(e);
            }
            
            foreach (var e in dialogs.Value)
            {
                ref var dialog = ref dialogs.Pools.Inc1.Get(e);
                if (!dialog.completed)
                {
                    if (gameUIManager.uiDialog.window.IsClosed())
                    {
                        var dialogData = dialog.dialogData;
                        
                        if (dialogData.output != null)
                        {
                            ref var inventory = ref dialog.sourceEntity.Get<InventoryComponent>();
                            
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
                        
                        dialog.completed = true;
                        dialogs.Pools.Inc2.Get(e).destroy = true;
                    }
                }
            }
            
            foreach (var e in interactableModels.Value)
            {
                var interactable = interactableModels.Pools.Inc1.Get(e);
                var model = interactableModels.Pools.Inc2.Get(e);
                var interactObject = model.modelGameObject.transform.FindInHierarchy("InteractHighlight");
                if (interactObject)
                {
                    interactObject.gameObject.SetActive(interactable.focusedByPlayer && interactable.showInteractBubble);
                }
            }
            
            foreach (var e in interactables.Value)
            {
                ref var interactable = ref interactables.Pools.Inc1.Get(e);
                interactable.focusedByPlayer = false;
            }
        }
    }
}