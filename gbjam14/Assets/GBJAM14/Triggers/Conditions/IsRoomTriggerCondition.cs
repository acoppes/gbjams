using System;
using Game;
using GBJAM14.Components;
using GBJAM14.Data;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Conditions
{
    public class IsRoomTriggerCondition : WorldTriggerCondition
    {
        public Room room;
        
        public override string GetObjectName()
        {
            if (room)
                return $"IsRoomActive({room.name})";
            return "IsRoomActive()";
        }

        public override bool Evaluate(object activator = null)
        {
            if (!room)
                return false;
            
            if (world.TryGetSingletonEntity<ActiveRoomComponent>(out var activeRoomEntity))
            {
                var activeRoom = activeRoomEntity.Get<ActiveRoomComponent>();
                return room.name.Equals(activeRoom.roomId, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }
    }
}