using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace GBJAM11.Components
{
    public struct JumpComponent : IEntityComponent
    {
        public enum State
        {
            None, Starting, Ending
        }
        
        public int durationInFrames;
        public int minDurationInFrames;
        public float initialSpeed;

        public State state;

        public float tempDrag;

        public int jumps;
        public int totalJumps;

        public bool jumpReleased;
    }
    
    public class JumpComponentDefinition : ComponentDefinitionBase
    {
        public int durationInFrames;
        public int minDurationInFrames;
        public float initialSpeed;
        public int totalJumps;
        
        public override string GetComponentName()
        {
            return nameof(JumpComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new JumpComponent()
            {
                durationInFrames = durationInFrames,
                minDurationInFrames = minDurationInFrames,
                initialSpeed = initialSpeed,
                totalJumps = totalJumps
            });
        }
    }
}