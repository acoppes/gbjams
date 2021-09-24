using System.Collections.Generic;
using System.Linq;
using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9
{
    public class Room : MonoBehaviour, IGameComponent
    {
        public RoomStart roomStart => GetComponentInChildren<RoomStart>();

        public List<RoomExit> roomExits => GetComponentsInChildren<RoomExit>().ToList();
    }
}
