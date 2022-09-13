using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class UnitStateComponent : MonoBehaviour, IEntityComponent
    {
        [NonSerialized]
        public bool walking;
        
        [NonSerialized]
        public bool chargeAttack1;
        
        [NonSerialized]
        public bool chargeAttack2;
        
        [NonSerialized]
        public bool kunaiAttacking;
        
        [NonSerialized]
        public bool swordAttacking;

        [NonSerialized]
        public bool dashing;

        [NonSerialized]
        public bool dead;
        
        [NonSerialized]
        public bool hit;
        
        [NonSerialized]
        public bool invulnerable;
        
        public static readonly int walkingStateHash = Animator.StringToHash("walking");
        public static readonly int dashingStateHash = Animator.StringToHash("dashing");
        public static readonly int kunaiAttackStateHash = Animator.StringToHash("kunai_attack");
        public static readonly int chargeAttack1StateHash = Animator.StringToHash("charge_attack1");
        public static readonly int chargeAttack2StateHash = Animator.StringToHash("charge_attack2");
        public static readonly int swordAttackStateHash = Animator.StringToHash("sword_attack");
        public static readonly int deadStateHash = Animator.StringToHash("dead");
        public static readonly int hittedStateHash = Animator.StringToHash("hitted");
        public static readonly int invulnerableStateHash = Animator.StringToHash("invulnerable");
    }
}