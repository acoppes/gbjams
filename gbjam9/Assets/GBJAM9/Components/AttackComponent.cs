using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class AttackComponent : MonoBehaviour, IEntityComponent
    {
        public WeaponData weaponData;
        public Transform attackAttachPoint;
        
        [NonSerialized]
        public float cooldown;

        [NonSerialized]
        public int extraDamage;
    }
}