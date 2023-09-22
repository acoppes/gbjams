using Gemserk.Leopotam.Ecs;
using UnityEngine.Serialization;

namespace GBJAM11.Components
{
    public struct RollComponent: IEntityComponent
    {
        public float duration;
        public float speedMultiplierAir;
        public float speedMultiplierGround;
    }
    
    public class RollComponentDefinition : ComponentDefinitionBase
    {
        public float duration;
        [FormerlySerializedAs("speedMultiplier")] 
        public float speedMultiplierAir;
        public float speedMultiplierGround;
        
        public override string GetComponentName()
        {
            return nameof(RollComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new RollComponent()
            {
                speedMultiplierAir = speedMultiplierAir,
                speedMultiplierGround = speedMultiplierGround,
                duration = duration
            });
        }
    }
}