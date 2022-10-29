using GBJAM.Commons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Beatemup.Ending
{
    public class EndingSceneController : MonoBehaviour
    {
        public VictorySequence victorySequence;

        public string nextScene;
    
        // Start is called before the first frame update
        void Start()
        {
            victorySequence.Restart();
        }

        // Update is called once per frame
        void Update()
        {
            if (victorySequence.completed)
            {
                if (GameboyInput.Instance.current.AnyButtonPressed())
                {
                    SceneManager.LoadScene(nextScene);
                }
            }
        }
    }
}