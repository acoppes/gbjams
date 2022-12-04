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
            TransitionToRoom,
            Victory,
            Defeat
        }

        public State state;
    }
}