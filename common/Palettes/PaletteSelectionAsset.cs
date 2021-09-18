using System;
using UnityEngine;

namespace GBJAM.Commons
{
    [CreateAssetMenu(menuName = "Palette Selection")]
    public class PaletteSelectionAsset : ScriptableObject
    {
        [SerializeField]
        private int defaultPalette;
        
        [NonSerialized]
        public int currentPalette;
        
        public Texture2D[] palettes;

        private void OnEnable()
        {
            currentPalette = defaultPalette;
        }

        public Texture2D GetCurrentPalette()
        {
            return palettes[currentPalette];
        }
    }
}