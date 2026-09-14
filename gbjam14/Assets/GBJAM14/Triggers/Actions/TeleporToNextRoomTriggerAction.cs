using System.Diagnostics;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using MyBox;

namespace GBJAM14.Triggers.Actions
{
    public class TeleporToNextRoomTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            NextRoom = 0,
            Custom = 1
        } 
        
        public TriggerTarget target;

        public ActionType actionType;

        [ConditionalField(nameof(actionType), false, ActionType.Custom)]
        public string roomId;
        
        [ConditionalField(nameof(actionType), false, ActionType.Custom)]
        public string startId;
        
        public override string GetObjectName()
        {
            if (actionType == ActionType.Custom)
            {
                return $"TeleportTo{actionType}({target}, {roomId}, {startId})";
            }
            return $"TeleportTo{actionType}({target})";
        }
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var targets = world.GetEntities(target, activator);

            if (actionType == ActionType.NextRoom)
            {
                foreach (var entity in targets)
                {
                    if (entity.Has<RoomNavigationComponent>())
                    {
                        ref var roomNavigation = ref entity.Get<RoomNavigationComponent>();
                        roomNavigation.roomId = roomNavigation.nextRoomId;
                        roomNavigation.startId = roomNavigation.nextStartId;
                    }
                }
            } else if (actionType == ActionType.Custom)
            {
                foreach (var entity in targets)
                {
                    if (entity.Has<RoomNavigationComponent>())
                    {
                        ref var roomNavigation = ref entity.Get<RoomNavigationComponent>();
                        roomNavigation.roomId = roomId;
                        roomNavigation.startId = startId;
                    }
                }
            }

            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}