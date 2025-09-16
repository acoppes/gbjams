using Gemserk.Leopotam.Ecs;

namespace GBJAM13.Components
{
    public struct GamepadRumbleComponent : IEntityComponent
    {
        public float instensityMultiplier;
        public float totalTime;
        public float currentTime;
        public bool active;
    }
    
    public class GamepadRumbleComponentDefinition : ComponentDefinitionBase
    {
        public float instensityMultiplier = 1;
        public float defaultTime = 1;

        public override void Apply(World world, Entity entity)
        {
            entity.Add(new GamepadRumbleComponent()
            {
                totalTime = defaultTime,
                instensityMultiplier = instensityMultiplier
            });
        }
    }
}