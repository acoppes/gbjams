using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace GBJAM10.Definitions
{
    public abstract class TargetEffectDefinition : MonoBehaviour, IEntityDefinition
    {
        public abstract void Apply(World world, Entity entity);
    }
}