using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct PlatesTrapComponent : IEntityComponent
    {
        public int pressCount;

        public bool wasPressed;
        
        // public bool disableAutomaticRestore;
        public float restoreTimeTotal;

        public float restoreTimeCurrent;
    }
    
    public class PlateTrapComponentDefinition : ComponentDefinitionBase, IEntityInstanceParameter
    {
        // public bool disableAutomaticRestore;
        public float timeToRestore;
        
        public override void Apply(World world, Entity entity)
        {
            entity.AddOrSet(new PlatesTrapComponent()
            {
                // disableAutomaticRestore = disableAutomaticRestore,
                restoreTimeTotal = timeToRestore
            });
        }
    }
}