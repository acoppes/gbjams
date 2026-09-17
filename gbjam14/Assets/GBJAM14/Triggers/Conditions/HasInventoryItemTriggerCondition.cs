using Game;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Conditions
{
    public class HasInventoryItemTriggerCondition : WorldTriggerCondition
    {
        public string inventoryItem;
        
        public override string GetObjectName()
        {
            return $"HasInventoryItem({inventoryItem})";
        }

        public override bool Evaluate(object activator = null)
        {
            if (world.TryGetSingletonEntity<MainCharacterComponent>(out var mainCharacterEntity))
            {
                var inventory = mainCharacterEntity.Get<InventoryComponent>();
                return inventory.items.Contains(inventoryItem);
            }

            return false;
        }
    }
}