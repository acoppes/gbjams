using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class DialogueComponent : MonoBehaviour, IEntityComponent
    {
        [NonSerialized]
        public bool visible;

        [NonSerialized] 
        public string text;
    }
}