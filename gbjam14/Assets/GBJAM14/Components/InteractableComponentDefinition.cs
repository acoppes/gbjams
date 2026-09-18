using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace GBJAM14.Components
{
    public struct InteractableFocusedByPlayerComponent : IEntityComponent
    {
        
    }
    
    public struct InteractableComponent : IEntityComponent
    {
        public bool showInteractBubble;
        public bool showFocusedUI;
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
        public bool showFocusedUI;
        
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new InteractableComponent()
            {
                showInteractBubble = showInteractBubble,
                showFocusedUI = showFocusedUI
            });
        }
    }
}