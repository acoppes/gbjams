using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GBJAM7.Scripts.MainMenu
{
    [Serializable]
    public class Level
    {
        public string name;
        public string sceneName;
    }
    
    public class MainMenuSceneController : MonoBehaviour
    {
        public OptionsMenu options;

        public GameboyButtonKeyMapAsset keyMapAsset;

        private bool showingOptions;

        public Level[] levels;

        public MainMenuIntro mainMenuIntro;

        private void Start()
        {
            options.title = "Pick stage";
        }

        public void Update()
        {
            keyMapAsset.UpdateControlState();

            if (!mainMenuIntro.completed)
            {
                if (keyMapAsset.AnyButtonPressed())
                {
                    mainMenuIntro.ForceComplete();
                    return;
                }
            }
            
            if (showingOptions)
            {

            } else {
                if (keyMapAsset.AnyButtonPressed())
                {
//                    SceneManager.LoadScene("GameScene");
//                    // TODO: HIDE PRESS START
//                    
                    showingOptions = true;
                    var optionsList = new List<Option>();
                    foreach (var level in levels)
                    {
                        optionsList.Add(new Option()
                        {
                            name = level.name
                        });
                    }
                    options.Show(optionsList, OnOptionSelected, OnCancel);
                }
            }
        }

        private void OnCancel()
        {
            options.Hide();
            showingOptions = false;
            
            // TODO: SHOW PRESS START
        }

        private void OnOptionSelected(int arg1, Option option)
        {
            foreach (var level in levels)
            {
                if (level.name.Equals(option.name))
                {
                    SceneManager.LoadScene(level.sceneName);
                }
            }

//            if ("New Game".Equals(option.name))
//            {
//                
//            }
//            
//            if ("Credits".Equals(option.name))
//            {
//                
//            }
        }
    }
}
