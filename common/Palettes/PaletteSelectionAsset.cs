using System;
using UnityEngine;

namespace GBJAM.Commons
{
    [CreateAssetMenu(menuName = "Palette Selection")]
    public class PaletteSelectionAsset : ScriptableObject
    {
        [SerializeField]
        public int defaultPalette;

        public Texture2D[] palettes;
    }
}