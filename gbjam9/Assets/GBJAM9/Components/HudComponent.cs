using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class HudComponent : MonoBehaviour, IEntityComponent
    {
        [NonSerialized]
        public bool visible = true;
    }
}