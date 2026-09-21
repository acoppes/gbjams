using System.Collections.Generic;
using Game;
using GBJAM14.Components;
using GBJAM14.Data;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

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
                foreach (var savedItem in saveGame.data.items)
                {
                    if (!mainCharacterEntity.Get<InventoryComponent>().items.Contains(savedItem))
                    {
                        mainCharacterEntity.Get<InventoryComponent>().items.Add(savedItem);
                    }
                }
                
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

        public static void ConfigureActiveRoomCameraConfiner(World world)
        {
            var virtualCameraObject = GameObject.Find("VirtualCamera");
            if (!virtualCameraObject)
            {
                return;
            }
            
            var worldVirtualCamera = virtualCameraObject.GetComponent<CinemachineVirtualCameraBase>();

            if (world.TryGetSingletonEntity<ActiveRoomComponent>(out var activeRoomEntity))
            {
                ref var activeRoom = ref activeRoomEntity.Get<ActiveRoomComponent>();

                var noRoomSet = !activeRoom.room && !string.IsNullOrEmpty(activeRoom.roomId);
                var roomChanged = activeRoom.room && !activeRoom.room.name.Equals(activeRoom.roomId);
                
                if (noRoomSet || roomChanged)
                {
                    var roomObject = GameObject.Find(activeRoom.roomId);
                    if (roomObject)
                    {
                        activeRoom.room = roomObject.GetComponent<Room>();
                        activeRoom.door = activeRoom.room.transform
                            .FindInHierarchy(activeRoom.doorId).GetComponent<RoomDoor>();
                    }
                }

                if (worldVirtualCamera && activeRoom.room)
                {
                    var confiner2D = worldVirtualCamera.GetComponent<CinemachineConfiner2D>();

                    if (!activeRoom.room.confiner)
                    {
                        var tilemapCollider2D = activeRoom.room.GetComponentInChildren<TilemapCollider2D>();
                        activeRoom.room.confiner = new GameObject();
                            
                        var bounds = tilemapCollider2D.bounds;
                        var boxCollider2D = activeRoom.room.confiner.AddComponent<BoxCollider2D>();
                        boxCollider2D.offset = bounds.center;
                        boxCollider2D.size = bounds.size;
                        
                        activeRoom.room.confiner.layer = LayerMask.NameToLayer("CameraConfiner");
                    }
                    
                    confiner2D.BoundingShape2D = activeRoom.room.confiner.GetComponent<BoxCollider2D>();
                }
            }
        }
    }
}