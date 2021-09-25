using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class GameComponent : MonoBehaviour, IEntityComponent
    {
        public enum State
        {
            Restarting,
            Playing, 
            Victory,
            Defeat
        }

        [NonSerialized]
        public State state;
    }
}