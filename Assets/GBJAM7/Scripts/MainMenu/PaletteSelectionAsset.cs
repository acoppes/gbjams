using UnityEngine;

namespace GBJAM7.Scripts.MainMenu
{
    [CreateAssetMenu(menuName = "Palette Selection")]
    public class PaletteSelectionAsset : ScriptableObject
    {
        public int currentPalette;
        public Texture2D[] palettes;

        public Texture2D GetCurrentPalette()
        {
            return palettes[currentPalette];
        }
    }
}