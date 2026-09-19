using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Conditions
{
    public class IsMainCharacterTriggerCondition : WorldTriggerCondition
    {
        public TriggerTarget target;
        
        public override string GetObjectName()
        {
            return "IsMainCharacter()";
        }

        public override bool Evaluate(object activator = null)
        {
            var entity = target.Get(world, activator);
                
            if (entity)
                return entity.Has<MainCharacterComponent>();

            return false;
        }
    }
}