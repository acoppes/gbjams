using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class GameComponent : MonoBehaviour, IEntityComponent
    {
        public enum State
        {
            Restarting,
            Fighting,
            WaitingReward,
            TransitioninToNextRoom,
            Victory,
            Defeat
        }

        [NonSerialized]
        public State state;
    }
}