using GBJAM7.Scripts;
using UnityEngine;

namespace Scenes.GameOverScene
{
    public class GameOverSceneController : MonoBehaviour
    {
        public GameOverController gameOverController;
        
        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                gameOverController.StartSequence(null, new GameOverData
                {
                    player1Defeated = false,
                    player2Defeated = true,
                    player1 = new PlayerData
                    {
                        name = "Player1"
                    },
                    player2 = new PlayerData
                    {
                        name = "Player2"
                    }
                });
            }
            
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                gameOverController.StartSequence(null, new GameOverData
                {
                    player1Defeated = true,
                    player2Defeated = false,
                    player1 = new PlayerData
                    {
                        name = "Player1"
                    },
                    player2 = new PlayerData
                    {
                        name = "Player2"
                    }
                });
            }
        }
    }
}
