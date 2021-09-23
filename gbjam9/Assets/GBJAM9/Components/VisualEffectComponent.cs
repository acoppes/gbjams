using System;
using GBJAM.Commons;
using UnityEngine;

namespace GBJAM9.Components
{
    public class VisualEffectComponent : MonoBehaviour, IGameComponent
    {
        public SfxVariant sfxVariant;

        [NonSerialized]
        public bool sfxSpawned;
    }
}