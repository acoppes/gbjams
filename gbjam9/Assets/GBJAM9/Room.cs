using UnityEngine;

namespace GBJAM9
{
    public class Room : MonoBehaviour
    {
        public RoomStart roomStart => GetComponentInChildren<RoomStart>();
    }
}
