using Game;
using GBJAM14.Components;
using GBJAM14.Data;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using Gemserk.Utilities;
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
                    Debug.Log($"Teleporting to [{activeRoom.roomId}, {activeRoom.doorId}]");
                    var room = GameObject.Find(activeRoom.roomId);
                    var doorTransform = room.transform.FindInHierarchy(activeRoom.doorId);
                    var door = doorTransform.GetComponent<RoomDoor>();
                    
                    // var door = activeRoom.door;
                    
                    entity.Get<PositionComponent>().value = door.enter.position;
                    entity.Get<LookingDirection>().value =
                        new Vector2(1, 0).Rotate(door.enter.localEulerAngles.z * Mathf.Deg2Rad);

                    if (entity.Has<RoomNavigationComponent>())
                    {
                        ref var roomNavigation = ref entity.Get<RoomNavigationComponent>();
                        if (!roomNavigation.visitedRooms.Contains(activeRoom.roomId))
                        {
                            roomNavigation.visitedRooms.Add(activeRoom.roomId);
                        }
                    }
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}