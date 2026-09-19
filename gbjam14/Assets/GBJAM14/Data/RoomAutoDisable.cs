using System;
using Game;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Signals;
using UnityEngine;

namespace GBJAM14.Data
{
    public class RoomAutoDisable : MonoBehaviour
    {
        // private Room room;
        public WorldReference worldReference;
        public SignalAsset onRoomInit;
        
        public void Awake()
        {
            onRoomInit.Register(OnRoomInit);
        }
        
        private void OnDestroy()
        {
            onRoomInit.Unregister(OnRoomInit);
        }

        // private void Start()
        // {
        //
        // }

        private void OnRoomInit(object userData)
        {
            var world = worldReference.GetReference(gameObject);
            
            if (world.TryGetSingletonEntity<ActiveRoomComponent>(out var activeRoomEntity))
            {
                var room = GetComponent<Room>();
                if (!room.name.Equals(activeRoomEntity.Get<ActiveRoomComponent>().roomId))
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}