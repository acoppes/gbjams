using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;

namespace GBJAM14.Triggers
{
    public class TeleporToNextRoomTriggerAction : WorldTriggerAction
    {
        public TriggerTarget target;

        // public RoomStartData roomStartData;
        
        public override string GetObjectName()
        {
            // var roomName = roomStartData ? roomStartData.name : string.Empty;
            return $"TeleportToRoomStart({target})";
        }
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var targets = world.GetEntities(target, activator);
                
            foreach (var entity in targets)
            {
                if (entity.Has<RoomNavigationComponent>())
                {
                    var roomNavigation = entity.Get<RoomNavigationComponent>();
                    entity.Get<PositionComponent>().value = roomNavigation.currentExit.roomStartData.transform.position;
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}