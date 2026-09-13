using System.Linq;
using GBJAM14.Components;
using GBJAM14.UI;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace GBJAM14.Systems
{
    public class InteractionSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
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

                if (interactAction.target.Has<NpcComponent>())
                {
                    ref var inventory = ref interactAction.source.Get<InventoryComponent>();
                    var npc = interactAction.target.Get<NpcComponent>();
                
                    var dialogData = characterDialogsDB.GetDialog(npc.characterId, inventory.items);

                    if (dialogData != null)
                    {
                        // add dialog to inventory to consider for other dialogs
                        inventory.items.Add(dialogData.id);

                        if (dialogData.status != null)
                        {
                            foreach (var statusId in dialogData.status)
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
                        
                        uiDialog.ShowDialog(dialogData);

                        var dialogEntity = world.CreateEntity();
                        dialogEntity.Add(new DialogComponent()
                        {
                            characterId = npc.characterId,
                            dialogId = dialogData.id,
                            completed = false
                        });   
                    }                    
                }

                if (interactAction.target.Has<ItemComponent>())
                {
                    ref var inventory = ref interactAction.source.Get<InventoryComponent>();
                    var item = interactAction.target.Get<ItemComponent>();
                
                    inventory.items.Add(item.itemId);

                    var dialogData = characterDialogsDB.GetDialog(item.itemId, inventory.items);

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
                
                    interactAction.target.Get<DestroyableComponent>().destroy = true;
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
        }


    }
}