using Game.DataAssets;
using UnityEditor;
using UnityEngine;
using File = System.IO.File;

public class PalleteToColorMapImporter
{
    public static class ConfigureFontTextureFilter
    {
        [MenuItem("GBJAM/Load Selected ColorSet From Palette")]
        public static void LoadColorSetFromPaletteFile()
        {
            var colorSet = Selection.activeObject as ColorSet;

            if (colorSet)
            { 
                var selectedFile = EditorUtility.OpenFilePanelWithFilters("Select Palette File", null, new[] { "Palette files", "pal" });

                if (!string.IsNullOrEmpty(selectedFile))
                {
                    var palContents = File.ReadAllLines(selectedFile);
                    
                    // var type = palContents[0];
                    // var config = palContents[1];
                    
                    Debug.Log($"Converting from palette {selectedFile} to selected colorSet");
                    
                    var colorsCount = int.Parse(palContents[2]);

                    colorSet.colors = new Color[colorsCount];
                    
                    for (var i = 0; i < colorsCount; i++)
                    {
                        var colors = palContents[i + 3].Split(' ');
                        colorSet.colors[i] = new Color(int.Parse(colors[0]) / 255f, int.Parse(colors[1]) / 255f, int.Parse(colors[2]) / 255f, 1f);
                    }
                    
                    EditorUtility.SetDirty(colorSet);
                }
            }
        }
    }
}
