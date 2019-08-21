using System.Collections;
using UnityEngine;

namespace GBJAM7.Scripts
{
    public class AttackSequence : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private float attackTime = 1;

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
