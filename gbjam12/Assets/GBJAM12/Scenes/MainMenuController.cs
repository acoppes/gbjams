using System.Collections;
using Game;
using UnityEngine;

namespace GBJAM12.Scenes
{
    public class MainMenuController : MonoBehaviour
    {
        // public string musicObjectName;
        
        public void FadeOutMusic()
        {
            StartCoroutine(FadeMusicAndDestroy());
        }

        private IEnumerator FadeMusicAndDestroy()
        {
            var bgMusic = FindAnyObjectByType<AudioSource>();
            if (bgMusic)
            {
                LeanTweenExtensions.fadeAudio(bgMusic, 1, 0, 0.5f);
                yield return new WaitForSeconds(0.5f);
                GameObject.Destroy(bgMusic.gameObject);
            }
        } 
    }
}
