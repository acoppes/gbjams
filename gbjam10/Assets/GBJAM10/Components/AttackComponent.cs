using System;
using UnityEngine;

namespace GBJAM10.Components
{
    public class AttackComponent : MonoBehaviour, IEntityComponent
    {
        public WeaponData weaponData;
        public Transform attackAttachPoint;
        
        [NonSerialized]
        public float cooldown;

        [NonSerialized]
        public int extraDamage;

        [NonSerialized]
        public Vector2 direction = Vector2.right;
    }
}