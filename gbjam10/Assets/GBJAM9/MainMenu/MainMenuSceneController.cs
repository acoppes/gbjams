using System.Collections;
using GBJAM.Commons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GBJAM7.Scripts.MainMenu
{
    public class MainMenuSceneController : MonoBehaviour
    {
        public MainMenuIntro mainMenuIntro;

        [SerializeField]
        private AudioSource startButtonSfx;

        private bool startedTransition = false;

        public float transitionDuration = 4.0f;

        private void Start()
        {
            mainMenuIntro.Next();
        }

        public void Update()
        {
            if (startedTransition)
            {
                return;
            }

            if (!mainMenuIntro.completed)
            {
                if (GameboyInput.Instance.current.AnyButtonPressed())
                {
                    mainMenuIntro.OnNextCompleted();
                }
            }
            else
            {
                StartCoroutine(SequenceToStartGame());
            }
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

                if (GameboyInput.Instance.current.AnyButtonPressed())
                {
                    transitionDuration = -1;
                }
            }
             
            SceneManager.LoadScene("Game");
        }
    }
}