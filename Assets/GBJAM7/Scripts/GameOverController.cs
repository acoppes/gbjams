using System.Collections;
using UnityEngine;

namespace GBJAM7.Scripts
{
    public struct GameOverData
    {
        public bool player1Defeated;
        public bool player2Defeated;

        public PlayerData player1;
        public PlayerData player2;
    }

    public class GameOverController : MonoBehaviour
    {
        public GameOverSequence sequence;
        
        public void StartSequence(GameController controller, GameOverData gameOverData)
        {
            StartCoroutine(GameOverSequence(controller, gameOverData));
        }
        
        private IEnumerator GameOverSequence(GameController controller, GameOverData gameOverData)
        {
            if (controller != null)
            {
                controller.HideMenus();
                controller.BlockPlayerActions();
            }
            
            sequence.SetGameOverData(gameOverData);
            sequence.StartSequence();
            
            yield return new WaitUntil(() => sequence.completed);

            // unlock player actions, if key pressed go to main menu
            // or show menu with restart or main menu
            
//            controller.UnblockPlayerActions)();
        }
    }
}