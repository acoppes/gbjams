using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Actions
{
    public class InteractTriggerAction : WorldTriggerAction
    {
        public TriggerTarget target;
        public TriggerTarget interactTarget;
        
        public override string GetObjectName()
        {
            return $"Interact({target}, {interactTarget})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var targetEntity = target.Get(world, activator);
            var interactEntity = interactTarget.Get(world, activator);
            
            if (interactEntity)
            {
                world.CreateEntity(null, null, (e) =>
                {
                    e.Add(new InteractActionComponent()
                    {
                        source = targetEntity,
                        target = interactEntity
                    });
                });
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}