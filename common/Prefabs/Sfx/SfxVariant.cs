using UnityEngine;

namespace GBJAM.Commons
{
    public class SfxVariant : MonoBehaviour
    {
        [SerializeField]
        protected AudioSource audioSource;

        [SerializeField]
        protected AudioClip[] variants;

        private int current;
        
        public void Play()
        {
            if (variants.Length == 0)
                return;
            
            audioSource.PlayOneShot(variants[current]);
            current = (current + 1) % variants.Length;
        }
    }
}
