using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GBJAM9
{
    public class Room : MonoBehaviour
    {
        public RoomStart roomStart => GetComponentInChildren<RoomStart>();

        public List<RoomExit> roomExits => GetComponentsInChildren<RoomExit>().ToList();
    }
}
