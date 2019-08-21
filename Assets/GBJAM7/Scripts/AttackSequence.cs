using System;
using System.Collections;
using UnityEngine;

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
        
        // public GameObject player1UnitPrefab;
        // public GameObject player2UnitPrefab;
    }
    
    // TODO: use a class to identify units in the hierarchy so we can easily remove them
    
    public class AttackSequence : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private float attackTime = 1;

        [SerializeField]
        private Transform[] player1UnitPositions;
        
        [SerializeField]
        private Transform[] player2UnitPositions;
        
        public void Show(AttackSequenceData attackData)
        {
            // destroy previous units
            
            // instantiate new units
            
            // start
            
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
            
            yield return new WaitForSeconds(attackTime);
            
            animator.SetBool("Player1AttackReady", true);
        }

        public void OnCameraInPosition2()
        {
            // play hit particles on enemies
            
            // kill enemies
          
            // if enemies can't attack back, then go to exit state
            
            // otherwise, attack back
            
            StartCoroutine(AttackPlayer2());
        }
        
        private IEnumerator AttackPlayer2()
        {
            // perform attacks!!
            
            // play each animation
            
            yield return new WaitForSeconds(attackTime);
            
            animator.SetBool("Player2AttackReady", true);
        }
        
        public void OnCameraInPosition3()
        {
            // play hit particles on units
            
            // kill units
            
            // complete sequence
        }
    }
}
