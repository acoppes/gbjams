using Game;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using UnityEngine;

namespace GBJAM14.Triggers.Actions
{
    public class TeleporToRoomStartTriggerAction : WorldTriggerAction
    {
        public TriggerTarget target;
        
        public override string GetObjectName()
        {
            return $"TeleportToRoomStart({target})";
        }
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var targets = world.GetEntities(target, activator);
               
            if (world.TryGetSingletonEntity<ActiveRoomComponent>(out var activeRoomEntity))
            {
                var activeRoom = activeRoomEntity.Get<ActiveRoomComponent>();
                
                foreach (var entity in targets)
                {
                    var room = GameObject.Find(activeRoom.roomId);
                    var start = room.transform.Find("Starts").Find(activeRoom.startId);
                    entity.Get<PositionComponent>().value = start.transform.position;
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}