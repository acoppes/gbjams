using System.Collections.Generic;
using System.Linq;
using GBJAM13.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GBJAM13.Scenes
{
    public class MainMenuController : MonoBehaviour
    {
        public int startingTotalJumps;

        [FormerlySerializedAs("onGameStarted")] 
        public UnityEvent onNewGameSelected;
        public UnityEvent onContinueGameSelected;
        
        public UIOptions options;
        
        public void StartGame()
        {

            // onGameStarted.Invoke();

            var optionNames = new List<string>()
            {
                "CONTINUE",
                "NEW GAME"
            };
            
            options.ShowOptions(optionNames.Select(n => new Option()
            {
                name = n, disabled = false, userData = null
            }).ToList());
            
            options.onOptionSelected.AddListener(OnOptionSelected);
        }

        private void OnOptionSelected()
        {
            if (options.selectedOptionIndex == 0)
            {
                // TODO: LOAD SAVEGAME FROM FILE
                GameParameters.saveGame = new SaveGame();
                onContinueGameSelected.Invoke();
            }
            else
            {
                GameParameters.saveGame = new SaveGame()
                {
                    resources = new int[]
                    {
                        0,
                        UnityEngine.Random.Range(5, 16),
                        UnityEngine.Random.Range(5, 11),
                        UnityEngine.Random.Range(10, 21),
                        UnityEngine.Random.Range(10, 21),
                        UnityEngine.Random.Range(5, 16),
                    }
                };
                
                // GameParameters.saveGame = new SaveGame()
                // {
                //     resources = new int[]
                //     {
                //        1, 1, 1, 1, 1
                //     }
                // };
                onNewGameSelected.Invoke();
            }
        }
    }
}
