using Gemserk.Leopotam.Ecs;

namespace GBJAM11.Components
{
    public struct ProjectileDamageComponent : IEntityComponent
    {
        public float damage;
    }
    
    public class ProjectileDamageComponentDefinition : ComponentDefinitionBase
    {
        public float damage;
        
        public override string GetComponentName()
        {
            return nameof(ProjectileDamageComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ProjectileDamageComponent()
            {
                damage = damage
            });
        }
    }
}