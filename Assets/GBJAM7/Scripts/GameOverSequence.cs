using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GBJAM7.Scripts
{
    public class GameOverSequence : MonoBehaviour
    {
        [NonSerialized]
        public bool completed;

        public Animator animator;

        public Text victoryPlayerText;
        public Text defeatPlayerText;

        public void SetGameOverData(GameOverData gameOverData)
        {
            // TODO: set the proper player name given victory or defeat
            
            victoryPlayerText.text = gameOverData.player1.name;
            defeatPlayerText.text = gameOverData.player2.name;
            
            // TODO: check victory player and flip, configure everything to show
            // inverted
        }

        public void StartSequence()
        {
            animator.SetTrigger("gameOver");
        }

        public void OnComplete()
        {
            completed = true;
        }

        public void ForceComplete()
        {
            animator.Play("Idle", -1, 0);
            completed = true;
        }
    }
}