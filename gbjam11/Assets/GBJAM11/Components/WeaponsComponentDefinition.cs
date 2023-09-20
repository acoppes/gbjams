using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace GBJAM11.Components
{
    public struct Weapon
    {
        public IEntityDefinition projectileDefinition;
        public IEntityDefinition directionIndicatorDefinition;
        public Entity directionIndicatorInstance;
    }
    
    public struct WeaponsComponent : IEntityComponent
    {
        public Weapon weapon;

        public Vector2 direction;
        
        // public Entity lastFiredProjectile;
    }
    
    public class WeaponsComponentDefinition : ComponentDefinitionBase
    {
        public Object projectileDefinition;
        public Object directionIndicatorDefinition;
        
        public override string GetComponentName()
        {
            return nameof(WeaponsComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new WeaponsComponent()
            {
                weapon = new Weapon()
                {
                    projectileDefinition = projectileDefinition.GetInterface<IEntityDefinition>(),
                    directionIndicatorDefinition = directionIndicatorDefinition.GetInterface<IEntityDefinition>(),
                    directionIndicatorInstance = Entity.NullEntity
                },
                // lastFiredProjectile = Entity.NullEntity
            });
        }
    }
}