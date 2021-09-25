using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GBJAM9.Components
{
    public class RoomComponent : MonoBehaviour, IEntityComponent
    {
        public enum State
        {
            Fighting = 0,
            WaitingReward = 1,
            Completed
        }
        
        public RoomStart roomStart => GetComponentInChildren<RoomStart>();

        public List<RoomExitSpawn> roomExits => GetComponentsInChildren<RoomExitSpawn>().ToList();
        
        public List<RoomEnemySpawn> roomSpawners => GetComponentsInChildren<RoomEnemySpawn>().ToList();
        
        public RoomRewardSpawn roomRewardSpawn => GetComponentInChildren<RoomRewardSpawn>();

        public AudioClip completedMusic;
        
        public AudioClip fightMusic;

        public State state = State.Fighting;

        [NonSerialized]
        public string rewardType;
    }
}
