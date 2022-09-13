using System;
using UnityEngine;

namespace GBJAM10.Components
{
    public class DialogueComponent : MonoBehaviour, IEntityComponent
    {
        [NonSerialized]
        public bool visible;

        [NonSerialized] 
        public string text;
    }
}