using System.Collections;
using UnityEngine;

namespace GBJAM7.Scripts
{
    public struct GameOverData
    {
        public bool player1Defeated;
        public bool player2Defeated;
    }

    // we are probably having a game over sequence class too to be called from game over controller
    
    public class GameOverController : MonoBehaviour
    {
        public GameOverSequence sequence;
        
        public void StartSequence(GameController controller, GameOverData gameOverData)
        {
            StartCoroutine(GameOverSequence(controller, gameOverData));
        }
        
        // TODO: sequence
        private IEnumerator GameOverSequence(GameController controller, GameOverData gameOverData)
        {
            controller.HideMenus();
            controller.BlockPlayerActions();

            sequence.StartSequence();
            
            yield return new WaitUntil(() => sequence.completed);

            // unlock player actions, if key pressed go to main menu
            // or show menu with restart or main menu
            
//            controller.UnblockPlayerActions)();
        }
    }
}