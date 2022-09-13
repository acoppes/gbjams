using System;
using GBJAM.Commons;
using UnityEngine;

namespace GBJAM10.Components
{
    public class SoundEffectComponent : MonoBehaviour, IEntityComponent
    {
        public SfxVariant sfx;
     
        [NonSerialized]
        public bool started;
    }
}