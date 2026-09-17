using Game;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace GBJAM14.Systems
{
    public class InventoryRequirementsSystem : BaseSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InventoryRequirementComponent, DestroyableComponent>, Exc<DisabledComponent>> 
            items = default;

        public void Run(EcsSystems systems)
        {
            if (world.TryGetSingletonEntity<MainCharacterComponent>(out var mainCharacterEntity))
            {
                var inventoryComponent = mainCharacterEntity.Get<InventoryComponent>();
                
                foreach (var e in items.Value)
                {
                    var requirements = items.Pools.Inc1.Get(e);
                    var meetsRequirements = true;
                    
                    foreach (var requirement in requirements.requirements)
                    {
                        var requirementName = requirement.Substring(1);
                        
                        if (requirement.StartsWith("+"))
                        {
                            if (!inventoryComponent.items.Contains(requirementName))
                            {
                                meetsRequirements = false;
                                Debug.Log($"Item didn't match: {requirement}");
                                break;
                            }
                        }
                        
                        if (requirement.StartsWith("-"))
                        {
                            if (inventoryComponent.items.Contains(requirementName))
                            {
                                meetsRequirements = false;
                                Debug.Log($"Item didn't match: {requirement}");
                                break;
                            }
                        }
                    }

                    if (!meetsRequirements)
                    {
                        Debug.Log($"deleting item, didn´t match requirements: {string.Join(',', requirements.requirements)}");
                        // items.Pools.Inc2.Get(e).destroy = true;
                        world.AddComponent(e, new DisabledComponent());
                    }
                }
            }
        }
    }
}