using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class RoomExitComponent : MonoBehaviour, IGameComponent
    {
        [NonSerialized]
        public bool mainUnitCollision;
    }
}