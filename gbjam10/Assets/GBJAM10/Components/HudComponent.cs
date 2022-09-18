using System;
using UnityEngine;

namespace GBJAM10.Components
{
    public class HudComponent : MonoBehaviour, IEntityComponent
    {
        [NonSerialized]
        public bool visible = true;
    }
}