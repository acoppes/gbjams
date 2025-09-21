using System.Collections.Generic;
using GBJAM13.UI;
using UnityEngine;
using UnityEngine.Events;

namespace GBJAM13.Scenes
{
    public class MainMenuController : MonoBehaviour
    {
        public int startingTotalJumps;

        public UnityEvent onGameStarted;

        public UIOptions options;
        
        public void StartGame()
        {

            // onGameStarted.Invoke();
            
            options.ShowOptions(new List<string>()
            {
                "CONTINUE",
                "NEW GAME"
            });
            
            options.onOptionSelected.AddListener(OnOptionSelected);
        }

        private void OnOptionSelected()
        {
            if (options.selectedOption == 0)
            {
                // TODO: LOAD SAVEGAME FROM FILE
                GameParameters.saveGame = new SaveGame();
                onGameStarted.Invoke();
            }
            else
            {
                GameParameters.saveGame = new SaveGame()
                {
                    resources = new int[]
                    {
                        UnityEngine.Random.Range(5, 16),
                        UnityEngine.Random.Range(5, 11),
                        UnityEngine.Random.Range(10, 21),
                        UnityEngine.Random.Range(10, 21),
                        UnityEngine.Random.Range(5, 16),
                    }
                };
                onGameStarted.Invoke();
            }
        }
    }
}
