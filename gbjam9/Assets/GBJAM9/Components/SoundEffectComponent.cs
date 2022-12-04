using System;
using GBJAM.Commons.Prefabs.Sfx;
using UnityEngine;

namespace GBJAM9.Components
{
    public class SoundEffectComponent : MonoBehaviour, IEntityComponent
    {
        public SfxVariant sfx;
     
        [NonSerialized]
        public bool started;
    }
}