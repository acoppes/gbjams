using System;
using System.Linq;
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

        public Transform[] elementsToInvert;

        public void SetGameOverData(GameOverData gameOverData)
        {
            if (gameOverData.defeatedPlayer == 1)
            {
                victoryPlayerText.text = gameOverData.player1.name;
                defeatPlayerText.text = gameOverData.player2.name;
                elementsToInvert.ToList().ForEach(t =>
                {
                    t.localScale = new Vector3(1, 1, 1);
                });
            }
            else
            {
                // TODO: invert all containers!!
                victoryPlayerText.text = gameOverData.player2.name;
                defeatPlayerText.text = gameOverData.player1.name;
                elementsToInvert.ToList().ForEach(t =>
                {
                    t.localScale = new Vector3(-1, 1, 1);
                });
            }
            
        }

        public void StartSequence()
        {
            completed = false;
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