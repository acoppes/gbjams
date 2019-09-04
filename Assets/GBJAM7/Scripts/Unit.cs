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

        public int squadSize = 3;
        
        public string name;
        public int cost;

        public bool isHero;
        public UnitType unitType;
        
        public int movementDistance;
        [FormerlySerializedAs("actionDistance")] 
        public int attackDistance;

        public int totalMovements = 1;
        public int currentMovements = 1;

        public float totalHP;
        public float currentHP;
        
        public int rangedDmg;
        public int meleeDmg;
        public int critChance;
        public int critMult;

        public int totalActions = 1;
        public int currentActions = 1;

        public int resources;

        public bool canCapture;

        public int regenHP = 0;

        public Vector3 moveDirection = new Vector3(1, 0, 0);

        public Animator animator;

        [Tooltip("The prefab to use for the ranged attack sequence")]
        public GameObject attackSequenceUnitPrefab;

        [NonSerialized]
        public int enemiesInRange;

        public void Death()
        {
            animator.SetBool("dead", true);
        }
    }
}
