using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Beatemup
{
    public static class WorldExtensions
    {
        public static Entity CreateEntity(this World world, GameObject definition)
        {
            return world.CreateEntity(definition.GetInterface<IEntityDefinition>());
        }
    }
}