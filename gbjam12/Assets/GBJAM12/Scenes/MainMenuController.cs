using System.Collections;
using Game;
using UnityEngine;

namespace GBJAM12.Scenes
{
    public class MainMenuController : MonoBehaviour
    {
        public string musicObjectName = "MainMenuMusic";
        
        public void FadeOutMusic()
        {
            StartCoroutine(FadeMusicAndDestroy());
        }

        private IEnumerator FadeMusicAndDestroy()
        {
            var musicGameObject = GameObject.Find(musicObjectName);
            if (musicGameObject)
            {
                var audioSource = musicGameObject.GetComponent<AudioSource>();
                LeanTweenExtensions.fadeAudio(audioSource, 1, 0, 0.5f);
                yield return new WaitForSeconds(0.5f);
                GameObject.Destroy(musicGameObject);
            }
        } 
    }
}
