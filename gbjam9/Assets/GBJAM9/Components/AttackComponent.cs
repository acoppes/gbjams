using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class AttackComponent : MonoBehaviour, IGameComponent
    {
        public WeaponData weaponData;
        public Transform attackAttachPoint;
        
        [NonSerialized]
        public float cooldown;
    }
}