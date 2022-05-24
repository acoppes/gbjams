using Gemserk.Leopotam.Ecs;
using UnityEngine;

public abstract class TargetEffectDefinition : MonoBehaviour, IEntityDefinition
{
    public abstract void Apply(World world, int entity);
}