using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using UnityEngine;

namespace GBJAM14.Triggers
{
    public class TeleporToNextRoomTriggerAction : WorldTriggerAction
    {
        public TriggerTarget target;
        
        public override string GetObjectName()
        {
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
                    var room = GameObject.Find(roomNavigation.nextRoomId);
                    var start = room.transform.Find("Starts").Find(roomNavigation.nextStartId);
                    entity.Get<PositionComponent>().value = start.transform.position;
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}