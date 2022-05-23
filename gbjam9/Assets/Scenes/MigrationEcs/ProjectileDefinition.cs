using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class ProjectileDefinition : MonoBehaviour, IEntityDefinition
{
    public float movementSpeed;
    public GameObject modelPrefab;

    public void Apply(World world, int entity)
    {
        world.AddComponent(entity, new UnitControlComponent());
        
        world.AddComponent(entity, new ProjectileComponent());
        world.AddComponent(entity, new PositionComponent());
        world.AddComponent(entity, new LookingDirection
        {
            value = Vector2.right,
            disableIndicator = true
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
    }
}