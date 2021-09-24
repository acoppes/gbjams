using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GBJAM9.Components
{
    public class ControllerComponent : MonoBehaviour, IGameComponent
    {
        public Object controllerObject;
        [NonSerialized]
        public bool initialized;
    }
}