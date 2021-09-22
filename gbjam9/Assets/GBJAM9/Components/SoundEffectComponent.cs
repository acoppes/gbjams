using System;
using GBJAM.Commons;
using UnityEngine;

namespace GBJAM9.Components
{
    public class SoundEffectComponent : MonoBehaviour, IGameComponent
    {
        public SfxVariant sfx;
     
        [NonSerialized]
        public bool started;
    }
}