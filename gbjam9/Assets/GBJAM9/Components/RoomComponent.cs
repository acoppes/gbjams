using System;
using System.Collections.Generic;
using System.Linq;
using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9
{
    public class RoomComponent : MonoBehaviour, IGameComponent
    {
        public enum State
        {
            Fighting = 0,
            WaitingReward = 1,
            Completed
        }
        
        public RoomStart roomStart => GetComponentInChildren<RoomStart>();

        public List<RoomExitSpawn> roomExits => GetComponentsInChildren<RoomExitSpawn>().ToList();
        
        public RoomRewardSpawn roomRewardSpawn => GetComponentInChildren<RoomRewardSpawn>();

        public AudioClip completedMusic;
        
        public AudioClip fightMusic;

        [NonSerialized]
        public State state = State.Fighting;

        [NonSerialized]
        public string rewardType;
    }
}
