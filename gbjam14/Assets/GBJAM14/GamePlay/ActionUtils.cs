using System.Collections.Generic;
using Game;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;

namespace GBJAM14.GamePlay
{
    public class ActionUtils
    {
        public static void SaveGameSave(World world)
        {
            var saveGame = SaveGame.saveGame;
            
            if (world.TryGetSingletonEntity<MainCharacterComponent>(out var mainCharacterEntity))
            {
                saveGame.data.items = new List<string>(mainCharacterEntity.Get<InventoryComponent>().items);
                saveGame.data.visitedRooms = new List<string>(mainCharacterEntity.Get<RoomNavigationComponent>().visitedRooms);
            }

            if (world.TryGetSingletonEntity<ActiveRoomComponent>(out var activeRoomEntity))
            {
                saveGame.data.roomData = new SavegameRoomData()
                {
                    currentRoom = activeRoomEntity.Get<ActiveRoomComponent>().roomId,
                    currentStart = activeRoomEntity.Get<ActiveRoomComponent>().doorId
                };
            }
            
            saveGame.Save();
        }

        public static void SaveGameLoad(World world)
        {
            var saveGame = SaveGame.saveGame;
            saveGame.Load();
            
            if (world.TryGetSingletonEntity<MainCharacterComponent>(out var mainCharacterEntity))
            {
                mainCharacterEntity.Get<InventoryComponent>().items = new List<string>(saveGame.data.items);
                mainCharacterEntity.Get<RoomNavigationComponent>().visitedRooms = new List<string>(saveGame.data.visitedRooms);
            }
            
            if (world.TryGetSingletonEntity<ActiveRoomComponent>(out var currentRoomEntity))
            {
                if (saveGame.data.roomData != null)
                {
                    currentRoomEntity.Get<ActiveRoomComponent>().roomId = saveGame.data.roomData.currentRoom;
                    currentRoomEntity.Get<ActiveRoomComponent>().doorId = saveGame.data.roomData.currentStart;
                }
            }
        }
    }
}