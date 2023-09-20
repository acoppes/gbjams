using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace GBJAM11.Components
{
    public struct WeaponComponent: IEntityComponent
    {
        public IEntityDefinition projectileDefinition;

        public Entity holder;
        public bool charging;
    }
    
    public class WeaponComponentDefinition : ComponentDefinitionBase
    {
        public Object projectileDefinition;
        
        public override string GetComponentName()
        {
            return nameof(WeaponsComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new WeaponComponent()
            {
                projectileDefinition = projectileDefinition.GetInterface<IEntityDefinition>(),
            });
        }
    }
}