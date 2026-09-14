using GBJAM14.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GBJAM14.Scenes
{
    public class MainMenuController : MonoBehaviour
    {
        [FormerlySerializedAs("onGameStarted")] 
        public UnityEvent onNewGameSelected;
        public UnityEvent onContinueGameSelected;
        
        public UIOptions options;
        
        public void StartGame()
        {
            
            onNewGameSelected.Invoke();
        }

        private void OnOptionSelected()
        {
            if (options.selectedOptionIndex == 0)
            {
                // TODO: LOAD SAVEGAME FROM FILE
                // GameParameters.saveGame = new SaveGame();
                onContinueGameSelected.Invoke();
            }
            else
            {
                // GameParameters.saveGame = new SaveGame() { };
                onNewGameSelected.Invoke();
            }
        }
    }
}
