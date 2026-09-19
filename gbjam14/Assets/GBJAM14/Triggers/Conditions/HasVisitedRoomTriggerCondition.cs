using Game;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Conditions
{
    public class HasVisitedRoomTriggerCondition : WorldTriggerCondition
    {
        public string roomId;
        
        public override string GetObjectName()
        {
            return $"HasVisitedRoom({roomId})";
        }

        public override bool Evaluate(object activator = null)
        {
            if (world.TryGetSingletonEntity<MainCharacterComponent>(out var mainCharacterEntity))
            {
                var roomNavigation = mainCharacterEntity.Get<RoomNavigationComponent>();
                return roomNavigation.visitedRooms.Contains(roomId.Trim());
            }

            return false;
        }
    }
}