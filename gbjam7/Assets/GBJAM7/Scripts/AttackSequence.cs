using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GBJAM7.Scripts.MainMenu;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GBJAM7.Scripts
{
    [Serializable]
    public struct AttackSequenceData
    {
        public int player1Units;
        public int player2Units;

        public int player1Killed;
        public int player2Killed;

        public int playerAttacking;

        public bool counterAttack;
        public int distance;
        
        public GameObject player1UnitPrefab;
        public GameObject player2UnitPrefab;

        public PlayerData player1Data;
        public PlayerData player2Data;

        public bool p1Crit;
        public bool p2Crit;
    }
    
    // TODO: use a class to identify units in the hierarchy so we can easily remove them
    
    public class AttackSequence : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private float attackTime = 1;
        
        [SerializeField]
        private float hitTime = 0.05f;
        
        [SerializeField]
        private int hitsPerUnit;

        [SerializeField]
        private Transform[] player1UnitPositions;
        
        [SerializeField]
        private Transform[] player2UnitPositions;

        private List<UnitBig> player1Units = new List<UnitBig>();
        private List<UnitBig> player2Units = new List<UnitBig>();

        private AttackSequenceData attackData;

        public bool completed;

        public Transform[] flipTransforms;

        public Text player1Info;
        public Text player2Info;

//        private GameboyButtonKeyMapAsset keyAsset;
//
//        private void Update()
//        {
//            if (keyAsset.)
//            if (Input.GetKeyDown(KeyCode.T))
//            {
//                Time.timeScale = 2;
//            }
//            
//            if (Input.GetKeyUp(KeyCode.T))
//            {
//                Time.timeScale = 1;
//            }
//        }

        public void Show(AttackSequenceData attackData)
        {
            completed = false;

            player1Info.text = attackData.player1Data.name;
            player2Info.text = attackData.player2Data.name;
            
            this.attackData = attackData;

            flipTransforms.ToList().ForEach(t => t.localScale = new Vector3(attackData.playerAttacking == 0 ? 1 : -1, 1, 1));

            player1Units.Clear();
            player2Units.Clear();

            for (var i = 0; i < player1UnitPositions.Length; i++)
            {
                for (var  j = 0; j < player1UnitPositions[i].childCount; j++)
                {
                    Destroy(player1UnitPositions[i].GetChild(j).gameObject);
                }
            }
            
            for (var i = 0; i < player2UnitPositions.Length; i++)
            {
                for (var  j = 0; j < player2UnitPositions[i].childCount; j++)
                {
                    Destroy(player2UnitPositions[i].GetChild(j).gameObject);
                }
            }
            
            for (var i = 0; i < attackData.player1Units; i++)
            {
                var unitObject = Instantiate(attackData.player1UnitPrefab, player1UnitPositions[i]);
                player1Units.Add(unitObject.GetComponentInChildren<UnitBig>());
            }

            for (var i = 0; i < attackData.player2Units; i++)
            {
                var unitObject = Instantiate(attackData.player2UnitPrefab, player2UnitPositions[i]);
                player2Units.Add(unitObject.GetComponentInChildren<UnitBig>());
            }
            
            // start
            
            animator.SetBool("Completed", false);
            animator.SetBool("Player1AttackReady", false);
            animator.SetBool("Player2AttackReady", false);
            animator.Play("ToPosition1", -1, 0);
        }

        public void OnCameraInPosition1()
        {
            // play attack on each unit...

            StartCoroutine(AttackPlayer1());
        }

        private IEnumerator AttackPlayer1()
        {
            // perform attacks!!
            
            // play each animation

            foreach (var unit in player1Units)
            {
                unit.attackingRanged = true;
//                unit.attackingRanged = attackData.distance > 1;
//                unit.criticalHit = attackData.p1Crit;
                unit.criticalHit = false;
                unit.StartAttacking();
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.2f));
            }

            yield return new WaitForSeconds(attackTime);
            
            foreach (var unit in player1Units)
            {
                unit.StopAttacking();
            }
            
            animator.SetBool("Player1AttackReady", true);
        }

        public void OnCameraInPosition2()
        {
            StartCoroutine(Player2HitSequence());
        }

        private IEnumerator Player2HitSequence()
        {
            // hit particles
            
            for (var i = 0; i < hitsPerUnit; i++)
            {
                foreach (var unit in player2Units)
                {
                    unit.ShowHitParticle();
                    yield return new WaitForSeconds(hitTime);
                }
            }

            for (var i = 0; i < attackData.player2Killed; i++)
            {
                if (i < player2Units.Count)
                { 
                    player2Units[i].Death();

                    yield return new WaitForSeconds(1);

                    Destroy(player2Units[i].gameObject);
                }
            }
            // kill enemies
          
            // if enemies can't attack back, then go to exit state

            if (attackData.player2Killed >= attackData.player2Units || !attackData.counterAttack)
            {
                animator.SetBool("Completed", true);
                yield break;
            }
            
            // otherwise, attack back
            
            StartCoroutine(AttackPlayer2());
        }
        
        private IEnumerator AttackPlayer2()
        {
            // perform attacks!!
            
            // play each animation
            
            foreach (var unit in player2Units)
            {
                if (unit == null || unit.gameObject == null)
                    continue;
                unit.attackingRanged = attackData.distance > 1;
                unit.criticalHit = attackData.p2Crit;
                unit.StartAttacking();
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.2f));
            }
            
            foreach (var unit in player2Units)
            {
                if (unit == null || unit.gameObject == null)
                    continue;
                unit.StopAttacking();
            }
            
            yield return new WaitForSeconds(attackTime);
            
            animator.SetBool("Player2AttackReady", true);
        }
        
        public void OnCameraInPosition3()
        {
            StartCoroutine(Player1HitSequence());
        }

        private IEnumerator Player1HitSequence()
        {
            // hit particles
            for (var i = 0; i < hitsPerUnit; i++)
            {
                foreach (var unit in player1Units)
                {
                    unit.ShowHitParticle();
                    yield return new WaitForSeconds(hitTime);
                }
            }
            
            // kill
            for (var i = 0; i < attackData.player1Killed; i++)
            {
                if (i < player1Units.Count)
                {
                    player1Units[i].Death();

                    yield return new WaitForSeconds(1);

                    Destroy(player1Units[i].gameObject);
                }
            }
            
            // complete sequence
            animator.SetBool("Completed", true);
        }

        public void ForceComplete()
        {
            animator.Play("Completed", -1, 0);
            animator.SetBool("Completed", true);
        }

        public void OnSequenceCompleted()
        {
            completed = true;
        }
    }
}
