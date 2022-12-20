using System;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class EntityReference : MonoBehaviour
    {
        [NonSerialized]
        public Entity entity;
    }
}