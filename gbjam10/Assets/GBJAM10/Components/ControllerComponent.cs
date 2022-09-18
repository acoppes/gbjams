using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GBJAM10.Components
{
    public class ControllerComponent : MonoBehaviour, IEntityComponent
    {
        public Object controllerObject;
        
        [NonSerialized]
        public bool initialized;
    }
}