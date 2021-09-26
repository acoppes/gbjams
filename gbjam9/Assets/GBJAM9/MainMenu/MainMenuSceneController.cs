using GBJAM.Commons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GBJAM7.Scripts.MainMenu
{
    public class MainMenuSceneController : MonoBehaviour
    {
        public GameboyButtonKeyMapAsset keyMapAsset;
        
        public MainMenuIntro mainMenuIntro;

        [SerializeField]
        private AudioSource startButtonSfx;

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
            
            if (keyMapAsset.AnyButtonPressed())
            {
                if (startButtonSfx != null)
                {
                    startButtonSfx.Play();
                }
                    
                mainMenuIntro.HideStart();

                SceneManager.LoadScene("Game");
                // ScenesLoader.LoadLevel(level);
            }
        }
    }
}