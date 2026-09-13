using GBJAM14.Components;
using GBJAM14.UI;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace GBJAM14.Systems
{
    public class CharacterInteractionSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<NpcComponent, CanBeInteractedComponent>, Exc<DisabledComponent>> 
            interactableCharacters = default;
        
        private readonly EcsFilterInject<Inc<PickupActionComponent>, Exc<DisabledComponent>> 
            pickupActions = default;

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
            foreach (var e in interactableCharacters.Value)
            {
                var character = interactableCharacters.Pools.Inc1.Get(e);
                ref var interacted = ref interactableCharacters.Pools.Inc2.Get(e);

                if (interacted.interactPending)
                {
                    interacted.interactPending = false;
                    
                    if (uiDialog.window.IsClosed())
                    {
                        var dialogData = characterDialogsDB.GetDialog(character.characterId);
                        uiDialog.ShowDialog(dialogData);

                        var dialogEntity = world.CreateEntity();
                        dialogEntity.Add(new DialogComponent()
                        {
                            characterId = character.characterId,
                            dialogId = dialogData.id,
                            completed = false
                        });
                    }
                    return;
                }
            }
            
            foreach (var e in pickupActions.Value)
            {
                var pickupAction = pickupActions.Pools.Inc1.Get(e);

                ref var inventory = ref pickupAction.picker.Get<InventoryComponent>();
                var item = pickupAction.pickup.Get<ItemComponent>();
                
                inventory.items.Add(item.itemId);

                var dialogData = characterDialogsDB.GetDialog(item.itemId);

                if (dialogData != null)
                {
                    uiDialog.ShowDialog(dialogData);

                    var dialogEntity = world.CreateEntity();
                    dialogEntity.Add(new DialogComponent()
                    {
                        characterId = item.itemId,
                        dialogId = dialogData.id,
                        completed = false
                    });   
                }
                
                pickupAction.pickup.Get<DestroyableComponent>().destroy = true;
                
                pickupActions.Pools.Inc1.Del(e);
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
        }


    }
}