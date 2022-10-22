using UnityEditor;
using UnityEngine;

namespace GBJAM10.Editor
{
    public static class ConfigureFontTextureFilter
    {
        [MenuItem("GBJAM/Configure Selected Font Texture Filter")]
        public static void ConfigureSelectedFontTextureFilter()
        {
            var font = Selection.activeObject as Font;
            if (font != null)
            {
                font.material.mainTexture.filterMode = FilterMode.Point;
            }
        }
    }
}