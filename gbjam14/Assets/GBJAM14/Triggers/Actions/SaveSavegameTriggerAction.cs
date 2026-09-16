using System.Collections.Generic;
using Game;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Actions
{
    public class SaveSavegameTriggerAction : WorldTriggerAction
    {
        public TriggerTarget target;
        
        public override string GetObjectName()
        {
            return $"Save({target})";
        }
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var playerEntity = target.Get(world, activator);
            
            var saveGame = SaveGame.saveGame;
            saveGame.data.items = new List<string>(playerEntity.Get<InventoryComponent>().items);
            
            if (world.TryGetSingletonEntity<ActiveRoomComponent>(out var activeRoomEntity))
            {
                saveGame.data.roomData = new SavegameRoomData()
                {
                    currentRoom = activeRoomEntity.Get<ActiveRoomComponent>().roomId,
                    currentStart = activeRoomEntity.Get<ActiveRoomComponent>().doorId
                };
            }
            
            saveGame.Save();
            return ITrigger.ExecutionResult.Completed;
        }
    }
}