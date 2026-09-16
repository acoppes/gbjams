using Game;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Utilities;
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
                return activeRoom.door;
            }

            return false;
        }
    }
}