using Gemserk.Leopotam.Ecs.Gameplay;

namespace Beatemup.Definitions
{
    public class GameTargetingDefinition : TargetingDefinition
    {
        public bool ignoreAlive;

        protected override TargetingParameters GetTargetingParameters()
        {
            return new TargetingParameters()
            {
                range = range,
                extra = ignoreAlive,
                extraValidation = delegate(TargetingParameters parameters, Target target)
                {
                    // var target.extra;
                
                    return true;
                }
            };
        }
    }
}