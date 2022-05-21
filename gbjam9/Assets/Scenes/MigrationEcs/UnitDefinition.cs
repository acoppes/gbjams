using System.Collections.Generic;
using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public class UnitDefinition : MonoBehaviour, IEntityDefinition
{
    public bool controllable;
    
    public float movementSpeed;
    public GameObject modelPrefab;

    public void Apply(World world, int entity)
    {
        world.AddComponent(entity, new PositionComponent());
        world.AddComponent(entity, new LookingDirection
        {
            value = Vector2.right
        });
        world.AddComponent(entity, new UnitControlComponent());
        
        if (controllable)
        {
            world.AddComponent(entity, new PlayerInputComponent());
        }
        
        world.AddComponent(entity, new UnitStateComponent());
        world.AddComponent(entity, new AnimatorComponent());
        
        world.AddComponent(entity, new StatesComponent());

        world.AddComponent(entity, new UnitModelComponent
        {
            prefab = modelPrefab
        });

        var abilityDefinitions = GetComponentsInChildren<AbilityDefinition>();
        var abilities = new List<Ability>();

        foreach (var abilityDefinition in abilityDefinitions)
        {
            abilities.Add(new Ability
            {
                name = abilityDefinition.gameObject.name,
                duration = abilityDefinition.duration,
                cooldown = abilityDefinition.cooldown
            });
        }
        
        world.AddComponent(entity, new AbilitiesComponent
        {
            abilities = abilities
        });

        var movementComponent = UnitMovementComponent.Default;
        movementComponent.speed = movementSpeed;
        world.AddComponent(entity, movementComponent);
    }
}