using System.Collections;
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

        private bool startedTransition = false;

        private float transitionDuration = 0.4f;

        public void Update()
        {
            keyMapAsset.UpdateControlState();

            if (startedTransition)
            {
                return;
            }
            
            if (!mainMenuIntro.completed)
            {
                if (keyMapAsset.AnyButtonPressed())
                {
                    mainMenuIntro.ForceComplete();
                    mainMenuIntro.HideStart();
                    StartCoroutine(SequenceToStartGame());
                }
            }
            else
            {
                StartCoroutine(SequenceToStartGame());
            }

            // if (keyMapAsset.AnyButtonPressed())
            // {
            //     if (startButtonSfx != null)
            //     {
            //         startButtonSfx.Play();
            //     }
            //         
            //     // mainMenuIntro.HideStart();
            //
            //     SceneManager.LoadScene("Game");
            //     // ScenesLoader.LoadLevel(level);
            // }
        }

        private IEnumerator SequenceToStartGame()
        {
            startedTransition = true;

            mainMenuIntro.visible = false;
            
            // wait some time, if pressed again, force complete that delay
            
            while (transitionDuration > 0)
            {
                yield return null;
                transitionDuration -= Time.deltaTime;

                if (keyMapAsset.AnyButtonPressed())
                {
                    transitionDuration = -1;
                }
            }
             
            SceneManager.LoadScene("Game");
        }
    }
}