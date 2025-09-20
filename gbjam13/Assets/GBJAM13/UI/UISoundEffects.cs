using Game.Components;
using MyBox;
using UnityEngine;

namespace GBJAM13.UI
{
    public class UISoundEffects : MonoBehaviour
    {
        public AudioSource audioSource;
        
        public void PlaySound(SoundEffectAsset asset)
        {
            if (!audioSource)
                return;
            
            audioSource.pitch = asset.randomPitch.RandomInRange();
            audioSource.clip = asset.clips.GetRandom();
            audioSource.volume = asset.volume;
            audioSource.outputAudioMixerGroup = asset.mixerGroup;
            audioSource.PlayOneShot(asset.clips.GetRandom());
        }
    }
}