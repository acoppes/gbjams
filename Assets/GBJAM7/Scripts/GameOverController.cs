using System.Collections;
using GBJAM7.Scripts.MainMenu;
using UnityEngine;

namespace GBJAM7.Scripts
{
    public struct GameOverData
    {
        public int defeatedPlayer;

        public PlayerData player1;
        public PlayerData player2;
    }

    public class GameOverController : MonoBehaviour
    {
        public GameOverSequence sequence;
        
        public GameboyButtonKeyMapAsset keyMapAsset;

        public bool inputEnabled;
        
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

            yield return null;
            
            inputEnabled = true;
            
            yield return new WaitUntil(() => sequence.completed);
        }

        private void Update()
        {
            if (!inputEnabled)
                return;

            if (sequence.completed)
            {
                if (keyMapAsset.AnyButtonPressed())
                {
                    ScenesLoader.ReturnToMainMenu();
                }
            }
            else
            {
                if (keyMapAsset.AnyButtonPressed())
                {
                    sequence.ForceComplete();
                }
            }
        }
    }
}