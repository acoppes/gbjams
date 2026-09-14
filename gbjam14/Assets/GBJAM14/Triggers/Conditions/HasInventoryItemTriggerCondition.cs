using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Conditions
{
    public class HasInventoryItemTriggerCondition : WorldTriggerCondition
    {
        public TriggerTarget target;
        public string inventoryItem;
        
        public override string GetObjectName()
        {
            return $"HasInventoryItem({target}, {inventoryItem})";
        }

        public override bool Evaluate(object activator = null)
        {
            var targetEntity = target.Get(world, activator);
            var inventory = targetEntity.Get<InventoryComponent>();

            return inventory.items.Contains(inventoryItem);
        }
    }
}