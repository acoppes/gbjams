using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GBJAM7.Scripts
{
    public class Unit : MonoBehaviour
    {
        public enum UnitType
        {
            Unit,
            Spawner,
            MainBase
        }

        public int player;
        
        public string name;

        public UnitType unitType;
        
        public int movementDistance;
        [FormerlySerializedAs("actionDistance")] public int attackDistance;

        public int totalMovements = 1;
        public int currentMovements = 1;

        public float totalHP;
        public float currentHP;
        
        public int dmg;

        public int totalActions = 1;
        public int currentActions = 1;

        public int resources;

        public Animator animator;

        private void LateUpdate()
        {
            animator.SetInteger("player", player);
        }
    }
}
