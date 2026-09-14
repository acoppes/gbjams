using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using UnityEngine;

namespace GBJAM14.Triggers.Conditions
{
    public class HasStoredRoomTriggerCondition : WorldTriggerCondition
    {
        public TriggerTarget target;

        public override string GetObjectName()
        {
            return $"HasStoredRoom({target})";
        }

        public override bool Evaluate(object activator = null)
        {
            var targetEntity = target.Get(world, activator);
            var roomNavigation = targetEntity.Get<RoomNavigationComponent>();

            if (string.IsNullOrEmpty(roomNavigation.roomId))
            {
                return false;
            }
            
            if (string.IsNullOrEmpty(roomNavigation.startId))
            {
                return false;
            }
            
            var room = GameObject.Find(roomNavigation.roomId);
            if (!room)
                return false;
            
            var start = room.transform.Find("Starts").Find(roomNavigation.startId);
            return start;
        }
    }
}