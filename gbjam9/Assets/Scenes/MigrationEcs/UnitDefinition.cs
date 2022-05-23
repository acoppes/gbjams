using System.Collections.Generic;
using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public class UnitDefinition : MonoBehaviour, IEntityDefinition
{
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
        world.AddComponent(entity, new PlayerInputComponent());
        
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
            IEntityDefinition projectileDefinition = null;

            if (abilityDefinition.projectileDefinitionPrefab != null)
            {
                projectileDefinition = abilityDefinition.projectileDefinitionPrefab
                    .GetComponentInChildren<IEntityDefinition>();
            }
            
            abilities.Add(new Ability
            {
                name = abilityDefinition.gameObject.name,
                duration = abilityDefinition.duration,
                cooldownTotal = abilityDefinition.cooldown,
                projectileDefinition = projectileDefinition
            });
        }
        
        world.AddComponent(entity, new AbilitiesComponent
        {
            abilities = abilities
        });

        world.AddComponent(entity, new UnitMovementComponent()
        {
            speed = movementSpeed
        });
        
        world.AddComponent(entity, new TargetComponent());
    }
}