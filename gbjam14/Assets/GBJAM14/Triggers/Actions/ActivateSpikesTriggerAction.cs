using System.Collections.Generic;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Actions
{
    public class ActivateSpikesTriggerAction : WorldTriggerAction
    {
        public TriggerTarget target;
        
        public override string GetObjectName()
        {
            return $"AcivateSpikes({target})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();
            target.Get(entities, world, activator);

            foreach (var entity in entities)
            {
                if (entity.Has<SpikesTrapComponent>())
                {
                    entity.Get<SpikesTrapComponent>().Activate();
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}