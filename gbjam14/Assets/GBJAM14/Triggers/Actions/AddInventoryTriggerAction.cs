using GBJAM14.Components;
using GBJAM14.GamePlay;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;

namespace GBJAM14.Triggers.Actions
{
    public class AddInventoryTriggerAction : WorldTriggerAction
    {
        public TriggerTarget target;
        public string itemId;

        public override string GetObjectName()
        {
            return $"AddInventory({target}, {itemId})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var targets = world.GetEntities(target, activator);
            foreach (var target in targets)
            {
                if (target.Has<InventoryComponent>())
                {
                    var inventoryComponent = target.Get<InventoryComponent>();
                    if (!inventoryComponent.items.Contains(itemId.Trim()))
                    {
                        inventoryComponent.items.Add(itemId.Trim());
                        ActionUtils.SaveGameSave(world);
                    }
                }
            }
            return ITrigger.ExecutionResult.Completed;
        }
    }
}