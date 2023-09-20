using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace GBJAM11.Components
{
    public struct WeaponsComponent : IEntityComponent
    {
        public IEntityDefinition defaultWeaponDefinition;
        public Entity weaponEntity;
        public bool inverted;
    }
    
    public class WeaponsComponentDefinition : ComponentDefinitionBase
    {
        public Object defaultWeaponDefinition;
        
        public override string GetComponentName()
        {
            return nameof(WeaponsComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new WeaponsComponent()
            {
                defaultWeaponDefinition =  defaultWeaponDefinition.GetInterface<IEntityDefinition>(),
                weaponEntity = Entity.NullEntity
            });
        }
    }
}