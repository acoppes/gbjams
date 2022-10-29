using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Beatemup.Definitions
{
    public abstract class TargetEffectDefinition : MonoBehaviour, IEntityDefinition
    {
        public abstract void Apply(World world, Entity entity);
    }
}