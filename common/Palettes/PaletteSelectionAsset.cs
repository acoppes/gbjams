using UnityEngine;

namespace GBJAM.Commons.Palettes
{
    [CreateAssetMenu(menuName = "Palette Selection")]
    public class PaletteSelectionAsset : ScriptableObject
    {
        [SerializeField]
        public int defaultPalette;

        public Texture2D[] palettes;
    }
}