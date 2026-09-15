using Game;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace GBJAM14.Systems
{
    public class InventoryRequirementsSystem : BaseSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ItemComponent, InventoryRequirementComponent, DestroyableComponent>, Exc<DisabledComponent>> 
            items = default;

        public void Run(EcsSystems systems)
        {
            if (world.TryGetSingletonEntity<MainCharacterComponent>(out var mainCharacterEntity))
            {
                var inventoryComponent = mainCharacterEntity.Get<InventoryComponent>();
                
                foreach (var e in items.Value)
                {
                    var requirements = items.Pools.Inc2.Get(e);
                    var meetsRequirements = true;
                    
                    foreach (var requirement in requirements.requirements)
                    {
                        if (requirement.StartsWith("+"))
                        {
                            meetsRequirements = meetsRequirements && inventoryComponent.items.Contains(requirement.Substring(1));
                        }
                        
                        if (requirement.StartsWith("-"))
                        {
                            meetsRequirements = meetsRequirements && !inventoryComponent.items.Contains(requirement.Substring(1));
                        }
                    }

                    if (!meetsRequirements)
                    {
                        items.Pools.Inc3.Get(e).destroy = true;
                    }
                }
            }
            

        }
    }
}