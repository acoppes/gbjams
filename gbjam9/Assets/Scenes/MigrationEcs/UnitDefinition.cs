using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class UnitDefinition : MonoBehaviour, IEntityDefinition
{
    public bool controllable;
    
    public float movementSpeed;
    public GameObject modelPrefab;
    
    public void Apply(World world, int entity)
    {
        world.AddComponent(entity, new PositionComponent());
        world.AddComponent(entity, new LookingDirection());
        world.AddComponent(entity, new UnitControlComponent());
        
        if (controllable)
        {
            world.AddComponent(entity, new PlayerInputComponent());
        }
        
        world.AddComponent(entity, new UnitStateComponent());
        world.AddComponent(entity, new AnimatorComponent());

        world.AddComponent(entity, new UnitModelComponent
        {
            prefab = modelPrefab
        });

        var movementComponent = UnitMovementComponent.Default;
        movementComponent.speed = movementSpeed;
        world.AddComponent(entity, movementComponent);
    }
}