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

        public UnitType unitType;
        
        public int movementDistance;
        [FormerlySerializedAs("actionDistance")] 
        public int attackDistance;

        public int totalMovements = 1;
        public int currentMovements = 1;

        public float totalHP;
        public float currentHP;
        
        public int dmg;

        public int totalActions = 1;
        public int currentActions = 1;

        public int resources;

        public bool canCapture;

        public int regenHP = 0;

        public Vector3 moveDirection = new Vector3(1, 0, 0);

        [Tooltip("The prefab to use for the attack sequence")]
        public GameObject attackSequenceUnitPrefab;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            var p = transform.position;
            transform.position = new Vector3(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y), Mathf.RoundToInt(p.z));
        }

        private void LateUpdate()
        {
            spriteRenderer.flipX = player != 0;
//            spriteRenderer.flipX = moveDirection.x < 0;
        }
    }
}
