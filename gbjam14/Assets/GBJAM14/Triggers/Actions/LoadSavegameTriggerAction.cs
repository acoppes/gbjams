using System.Collections.Generic;
using Game;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Actions
{
    public class LoadSavegameTriggerAction : WorldTriggerAction
    {
        public TriggerTarget target;

        public override string GetObjectName()
        {
            return $"Load({target})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var playerEntity = target.Get(world, activator);
            
            var saveGame = SaveGame.saveGame;
            saveGame.Load();
            
            playerEntity.Get<InventoryComponent>().items = new List<string>(saveGame.data.items);
            
            // playerEntity.Get<RoomNavigationComponent>().roomId = saveGame.data.roomData.currentRoom;
            // playerEntity.Get<RoomNavigationComponent>().startId = saveGame.data.roomData.currentStart;

            if (world.TryGetSingletonEntity<ActiveRoomComponent>(out var currentRoomEntity))
            {
                if (saveGame.data.roomData != null)
                {
                    currentRoomEntity.Get<ActiveRoomComponent>().roomId = saveGame.data.roomData.currentRoom;
                    currentRoomEntity.Get<ActiveRoomComponent>().doorId = saveGame.data.roomData.currentStart;
                }
            }

            return ITrigger.ExecutionResult.Completed;
        }
    }
}