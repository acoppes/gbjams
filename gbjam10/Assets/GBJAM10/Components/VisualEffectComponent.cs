using System;
using GBJAM.Commons;
using UnityEngine;

namespace GBJAM10.Components
{
    public class VisualEffectComponent : MonoBehaviour, IEntityComponent
    {
        public SfxVariant sfxVariant;

        [NonSerialized]
        public bool sfxSpawned;
    }
}