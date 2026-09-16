using Game;
using GBJAM14.Components;
using GBJAM14.Data;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using Leopotam.EcsLite;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GBJAM14.Systems
{
    public class ActiveRoomSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        // cache camera
        private CinemachineVirtualCameraBase worldVirtualCamera;
        
        public void Init(EcsSystems systems)
        {
            var virtualCameraObject = GameObject.Find("VirtualCamera");
            if (virtualCameraObject)
            {
                worldVirtualCamera = virtualCameraObject.GetComponent<CinemachineVirtualCameraBase>();
            }
        }
        
        public void Run(EcsSystems systems)
        {
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
                    if (!confiner2D.BoundingShape2D)
                    {
                        var tilemapCollider2D = activeRoom.room.GetComponentInChildren<TilemapCollider2D>();
                        if (tilemapCollider2D)
                        {
                            activeRoom.room.confiner = new GameObject();
                            // activeRoom.room.confiner.transform.SetParent(activeRoom.room.transform, true);
                            
                            var bounds = tilemapCollider2D.bounds;
                            var boxCollider2D = activeRoom.room.confiner.AddComponent<BoxCollider2D>();
                            boxCollider2D.offset = bounds.center;
                            boxCollider2D.size = bounds.size;
                            confiner2D.BoundingShape2D = boxCollider2D;

                            activeRoom.room.confiner.layer = LayerMask.NameToLayer("CameraConfiner");
                        }
                    }
                }
            }
        }
    }
}