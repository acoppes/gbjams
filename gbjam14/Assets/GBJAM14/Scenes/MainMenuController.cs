using System.Collections.Generic;
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

        public string initialRoom;
        public string initialStart;
        
        public void StartGame()
        {
            var savegame = SaveGame.saveGame;
            savegame.Load();
            
            options.ShowOptions(new List<Option>()
            {
                new Option()
                {
                    name = "Continue",
                    disabled = savegame.data.roomData == null
                },
                new Option()
                {
                    name = "New Game",
                    disabled = false
                },
                new Option()
                {
                    name = "Exit Game",
                    disabled = false
                }
            });
            
            options.onOptionSelected.AddListener(OnOptionSelected);
        }
        
        private void OnOptionSelected()
        {
            if (options.selectedOptionIndex == 0)
            {
                options.window.Close();
                onContinueGameSelected.Invoke();
            }
            else if (options.selectedOptionIndex == 1)
            {
                var savegame = SaveGame.saveGame;
                savegame.Delete();
                savegame.data = new SavegameData()
                {
                    items = new List<string>(),
                    roomData = new SavegameRoomData()
                    {
                        currentRoom = initialRoom,
                        currentStart = initialStart
                    }
                };
                
                options.window.Close();
                onNewGameSelected.Invoke();
            } else if (options.selectedOptionIndex == 2)
            {
                Application.Quit(0);
            }
        }
    }
}
