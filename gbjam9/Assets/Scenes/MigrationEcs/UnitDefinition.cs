using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Extensions;
using UnityEngine;

public class UnitDefinition : MonoBehaviour, IEntityDefinition
{
    public float movementSpeed;
    public GameObject modelPrefab;
    
    public void Apply(World world, int entity)
    {
        world.AddComponent(entity, new PositionComponent
        {
            
        });
        
        world.AddComponent(entity, new UnitInputComponent
        {
            
        });
        
        world.AddComponent(entity, new UnitModelComponent
        {
            prefab = modelPrefab
        });

        var movementComponent = UnitMovementComponent.Default;
        movementComponent.speed = movementSpeed;
        world.AddComponent(entity, movementComponent);
    }
}