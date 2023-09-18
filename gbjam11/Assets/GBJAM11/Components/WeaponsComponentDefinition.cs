using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace GBJAM11.Components
{
    public struct Weapon
    {
        public IEntityDefinition projectileDefinition;
    }
    
    public struct WeaponsComponent : IEntityComponent
    {
        public Weapon weapon;
    }
    
    public class WeaponsComponentDefinition : ComponentDefinitionBase
    {
        public Object projectileDefinition;
        
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
                    projectileDefinition = projectileDefinition.GetInterface<IEntityDefinition>()
                }
            });
        }
    }
}