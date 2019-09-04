using UnityEngine;

namespace GBJAM7.Scripts.MainMenu
{
    public class BackgroundMusicSingleton : MonoBehaviour
    {
        private static BackgroundMusicSingleton instance;

        public AudioSource bgMusic;
        
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
            bgMusic.Play();
        }
    }
}
