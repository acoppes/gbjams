using System.Collections.Generic;
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
        
        private readonly EcsFilterInject<Inc<DialogComponent>, Exc<DisabledComponent>> 
            dialogs = default;
        
        private CharacterDialogsDB characterDialogsDB;
        private UIDialog uiDialog;
        
        public void Init(EcsSystems systems)
        {
            characterDialogsDB = FindFirstObjectByType<CharacterDialogsDB>();
            uiDialog = FindFirstObjectByType<UIDialog>();
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in interactActions.Value)
            {
                var interactAction = interactActions.Pools.Inc1.Get(e);

                if (interactAction.target.Has<CharacterComponent>())
                {
                    ref var inventory = ref interactAction.source.Get<InventoryComponent>();
                    ref var characterA = ref interactAction.source.Get<CharacterComponent>();

                    var characterB = interactAction.target.Get<CharacterComponent>();
                
                    var dialogs = characterDialogsDB.GetCharacterDialogs(characterB.characterId, 
                        inventory.items);

                    if (dialogs.Count > 0)
                    {
                        var dialogData = dialogs.GetRandom();
                        
                        // add dialog to inventory to consider for other dialogs
                        // inventory.items.Add(dialogData.id);

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
                        
                        uiDialog.ShowDialog(dialogData, new List<string>()
                        {
                            characterA.characterId,
                            characterB.characterId
                        });

                        var dialogEntity = world.CreateEntity();
                        dialogEntity.Add(new DialogComponent()
                        {
                            characterId = characterB.characterId,
                            dialogId = dialogData.id,
                            completed = false
                        });   
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
                    if (uiDialog.window.IsClosed())
                    {
                        dialog.completed = true;
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