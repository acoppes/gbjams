using Game;
using Game.Components;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace GBJAM14.Systems
{
    public class QuestIndicatorSystem : BaseSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<QuestsComponent, ModelInstanceComponent, InteractableComponent>, Exc<DisabledComponent, InteractableFocusedByPlayerComponent>> 
            questModelFilter = default;
        
        public void Run(EcsSystems systems)
        {
            if (world.TryGetSingletonEntity<MainCharacterComponent>(out var mainCharacterEntity))
            {
                var inventory = mainCharacterEntity.Get<InventoryComponent>();
                
                foreach (var e in questModelFilter.Value)
                {
                    var quests = questModelFilter.Pools.Inc1.Get(e);
                    var model = questModelFilter.Pools.Inc2.Get(e);
                    var interactable = questModelFilter.Pools.Inc3.Get(e);
                    
                    var interactObject = model.modelGameObject.transform.FindInHierarchy("Quest");
                    interactObject.localPosition = new Vector3(0, interactable.bubbleOffset, 0);

                    var shouldShowQuest = false;
                    
                    foreach (var quest in quests.quests)
                    {
                        if (inventory.items.Contains(quest))
                        {
                            shouldShowQuest = true;
                        }
                    }
                    
                    interactObject.gameObject.SetActive(shouldShowQuest);
                }
            }
        }
    }
}