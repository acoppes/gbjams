using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GBJAM7.Scripts.MainMenu
{
    public class MainMenuSceneController : MonoBehaviour
    {
        public OptionsMenu options;

        public GameboyButtonKeyMapAsset keyMapAsset;

        private bool showingOptions;
        
        public void Update()
        {
            keyMapAsset.UpdateControlState();
            
            if (showingOptions)
            {

            } else {
                if (keyMapAsset.AnyButtonPressed())
                {
                    options.Show(new List<Option>()
                    {
                        new Option { name = "New Game"},
                        new Option { name = "Credits"}
                    }, OnOptionSelected, OnCancel);
                }
            }
        }

        private void OnCancel()
        {
            options.Hide();
            showingOptions = false;
        }

        private void OnOptionSelected(int arg1, Option option)
        {
            if ("New Game".Equals(option.name))
            {
                
            }
        }
    }
}
