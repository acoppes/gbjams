using UnityEngine;
using UnityEngine.UI;

namespace GBJAM12.Scenes
{
    public class LoadLevelController : MonoBehaviour
    {
        public GameConfiguration gameConfiguration;

        public Text levelNumberText;
        public Text levelNameText;
        
        private void Start()
        {
            levelNumberText.text = $"LEVEL {GameController.currentLevel+1}";
            levelNameText.text = gameConfiguration.levels[GameController.currentLevel].name.ToUpper();
        }
    }
}