using System.Collections.Generic;
using UnityEngine;

namespace GBJAM.Commons
{
    public class GameboyInput : SingletonBehaviour<GameboyInput>
    {
        private static int currentIndex;
     
        public KeyCode switchConfigurationKey;
        
        public List<GameboyButtonKeyMapAsset> controlConfigurations;

        public GameboyButtonKeyMapAsset current => 
            currentIndex < controlConfigurations.Count ? controlConfigurations[currentIndex] : null;

        private void Update()
        {
            if (currentIndex < controlConfigurations.Count)
            {
                current.UpdateControlState();    
            }

            if (Input.GetKeyUp(switchConfigurationKey))
            {
                currentIndex = (currentIndex + 1) % controlConfigurations.Count;
            }
        }
    }
}