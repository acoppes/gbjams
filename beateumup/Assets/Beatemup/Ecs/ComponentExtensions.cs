using Gemserk.Leopotam.Ecs.Controllers;

namespace Beatemup.Ecs
{
    public static class ComponentExtensions
    {
        public static bool TryGetState(this StatesComponent statesComponent, string stateName, out State state)
        {
            return statesComponent.states.TryGetValue(stateName, out state);
        }
    }
}