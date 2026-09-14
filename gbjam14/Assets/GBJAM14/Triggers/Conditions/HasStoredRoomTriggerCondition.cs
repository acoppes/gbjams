using Game;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using UnityEngine;

namespace GBJAM14.Triggers.Conditions
{
    public class HasStoredRoomTriggerCondition : WorldTriggerCondition
    {
        public override string GetObjectName()
        {
            return "HasActiveRoom()";
        }

        public override bool Evaluate(object activator = null)
        {
            if (world.TryGetSingletonEntity<ActiveRoomComponent>(out var activeRoomEntity))
            {
                var activeRoom = activeRoomEntity.Get<ActiveRoomComponent>();
                
                if (string.IsNullOrEmpty(activeRoom.roomId))
                {
                    return false;
                }
            
                if (string.IsNullOrEmpty(activeRoom.startId))
                {
                    return false;
                }
            
                var room = GameObject.Find(activeRoom.roomId);
                if (!room)
                    return false;
            
                var start = room.transform.Find("Starts").Find(activeRoom.startId);
                return start;
            }

            return false;
        }
    }
}