using UnityEngine;

namespace GBJAM7.Scripts.MainMenu
{
    [CreateAssetMenu(menuName = "Palette Selection")]
    public class PaletteSelectionAsset : ScriptableObject
    {
        public Texture2D[] palettes;
    }
}