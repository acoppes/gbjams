using System.Collections.Generic;
using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class ProjectileDefinition : MonoBehaviour, IEntityDefinition
{
    public float movementSpeed;
    public GameObject modelPrefab;

    public void Apply(World world, int entity)
    {
        world.AddComponent(entity, new PlayerComponent());
        world.AddComponent(entity, new UnitControlComponent());
        
        world.AddComponent(entity, new ProjectileComponent());
        world.AddComponent(entity, new PositionComponent());
        world.AddComponent(entity, new LookingDirection
        {
            value = Vector2.right,
            disableIndicator = true
        });
        
        world.AddComponent(entity, new AbilitiesComponent
        {
            abilities = new List<Ability>(),
            targetings = new List<Targeting>()
        });
        
        world.AddComponent(entity, new UnitModelComponent
        {
            prefab = modelPrefab,
            rotateToDirection = true
        });
        
        world.AddComponent(entity, new UnitMovementComponent
        {
            speed = movementSpeed
        });
        
        world.AddComponent(entity, new TargetEffectsComponent
        {
            targetEffects = new List<ITargetEffect>()
        });
        
        world.AddComponent(entity, new HealthComponent
        {
            current = 1,
            total = 1
        });
    }
}