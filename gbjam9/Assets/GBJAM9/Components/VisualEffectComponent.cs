using System;
using GBJAM.Commons.Prefabs.Sfx;
using UnityEngine;

namespace GBJAM9.Components
{
    public class VisualEffectComponent : MonoBehaviour, IEntityComponent
    {
        public SfxVariant sfxVariant;

        [NonSerialized]
        public bool sfxSpawned;
    }
}