using System;
using GBJAM.Commons;
using UnityEngine;

namespace GBJAM9
{
    public class SoundEffect : MonoBehaviour
    {
        public SfxVariant sfx;
     
        [NonSerialized]
        public bool started;
    }
}