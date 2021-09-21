using System;
using UnityEngine;

namespace GBJAM9
{
    public class UnitState : MonoBehaviour
    {
        public enum State
        {
            Idle, Walking, Dashing, Death
        }

        [NonSerialized]
        public State state = State.Idle;
    }
}