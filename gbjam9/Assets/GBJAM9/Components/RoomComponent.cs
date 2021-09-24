using System.Collections.Generic;
using System.Linq;
using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9
{
    public class RoomComponent : MonoBehaviour, IGameComponent
    {
        public RoomStart roomStart => GetComponentInChildren<RoomStart>();

        public List<RoomExitSpawn> roomExits => GetComponentsInChildren<RoomExitSpawn>().ToList();

        public AudioClip completedMusic;
        
        public AudioClip fightMusic;
    }
}
