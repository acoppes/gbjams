using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace GBJAM14.Components
{
    public struct InteractableComponent : IEntityComponent
    {
        public bool focusedByPlayer;
        public bool showInteractBubble;
    }
    
    public struct InteractActionComponent : IActionComponent
    {
        public Entity source;
        public Entity target;
        public string optionalDialogId;
    }
    
    public class InteractableComponentDefinition : ComponentDefinitionBase
    {
        public bool showInteractBubble;
        
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new InteractableComponent()
            {
                showInteractBubble = showInteractBubble
            });
        }
    }
}