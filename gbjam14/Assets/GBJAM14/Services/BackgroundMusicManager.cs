using System.Collections;
using Game;
using UnityEngine;

namespace GBJAM14.Services
{
    public class BackgroundMusicManager : MonoBehaviour
    {
        private static GameObject BackgroundMusicInstance;
        
        public GameObject backgroundMusicPrefab;
        
        public AudioClip townMusic;
        public AudioClip templeMusic;

        public void PlayTown()
        {
            if (!BackgroundMusicInstance)
            {
                BackgroundMusicInstance = Instantiate(backgroundMusicPrefab);
                DontDestroyOnLoad(BackgroundMusicInstance);
            }
            
            var source = BackgroundMusicInstance.GetComponent<AudioSource>();
            if (source.clip != townMusic)
            {
                StartCoroutine(SwitchMusic(townMusic));
            }
        }
        
        public void PlayTemple()
        {
            if (!BackgroundMusicInstance)
            {
                BackgroundMusicInstance = Instantiate(backgroundMusicPrefab);
                DontDestroyOnLoad(BackgroundMusicInstance);
            }
            
            var source = BackgroundMusicInstance.GetComponent<AudioSource>();
            if (source.clip != templeMusic)
            {
                StartCoroutine(SwitchMusic(templeMusic));
            }
        }

        private IEnumerator SwitchMusic(AudioClip newAudioClip)
        {
            var source = BackgroundMusicInstance.GetComponent<AudioSource>();

            if (source.clip)
            {
                LeanTweenExtensions.fadeAudio(source, source.volume, 0, 0.25f);
                yield return new WaitForSecondsRealtime(0.25f);
            }
            
            // could use 2 clips and do some cross fade...
            
            source.clip = newAudioClip;
            source.Play();
            
            if (source.clip)
            {
                LeanTweenExtensions.fadeAudio(source, source.volume, 1f, 0.5f);
            }
        }
    }
}