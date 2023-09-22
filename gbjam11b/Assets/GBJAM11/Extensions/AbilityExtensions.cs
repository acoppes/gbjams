using System.Collections.Generic;
using Game.Components.Abilities;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;

namespace GBJAM11.Extensions
{
    public static class AbilityExtensions
    {
        public static bool CalculateTargets(this Ability ability, World world)
        {
            var targets = new List<Target>();
            var count = world.GetTargets(ability.GetRuntimeTargetingParameters(), targets);

            if (count > 0)
            {
                ability.CopyTargets(targets);
            }
            
            return count > 0;
        }
    }
}