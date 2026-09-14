using Game;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
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
            var targetEntity = target.Get(world, activator);
            
            if (world.TryGetSingletonEntity<ActiveRoomComponent>(out var activeRoomEntity))
            {
                ref var activeRoom = ref activeRoomEntity.Get<ActiveRoomComponent>();
                activeRoom.roomId = roomId;
                activeRoom.startId = startId;
                
                if (actionType == ActionType.NextRoom)
                {
                    if (targetEntity.Has<RoomNavigationComponent>())
                    {
                        var roomNavigation = targetEntity.Get<RoomNavigationComponent>();
                        activeRoom.roomId = roomNavigation.nextRoomId;
                        activeRoom.startId = roomNavigation.nextStartId;
                    }
                } else if (actionType == ActionType.Custom)
                {
                    activeRoom.roomId = roomId;
                    activeRoom.startId = startId;
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}