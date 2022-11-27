using System;
using Beatemup.Ecs;
using UnityEngine;

namespace Beatemup.Models
{
    public class TargetReference : MonoBehaviour
    {
        [NonSerialized]
        public TargetingUtils.Target target = new TargetingUtils.Target();
    }
}