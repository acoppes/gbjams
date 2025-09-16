using Game.DataAssets;
using UnityEditor;
using UnityEngine;
using File = System.IO.File;

namespace GBJAM13.Editor
{
    public static class PalleteToColorMapImporter
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
                    CopyPalFileContentsToColorSet(colorSet, selectedFile);
                    EditorUtility.SetDirty(colorSet);
                }
            }
        }
        
        [MenuItem("GBJAM/Save Selected ColorSet To Palette")]
        public static void SaveColorSetToPaletteFile()
        {
            var colorSet = Selection.activeObject as ColorSet;

            if (colorSet)
            { 
                var selectedFileDestination = EditorUtility.SaveFilePanel("Select Destination", null, $"{colorSet.name}",  "pal");

                if (!string.IsNullOrEmpty(selectedFileDestination))
                {
                    SaveColorSetToPalFile(colorSet, selectedFileDestination);
                    EditorUtility.SetDirty(colorSet);
                }
            }
        }

        public static void CopyPalFileContentsToColorSet(ColorSet colorSet, string selectedFile)
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
                colorSet.colors[i] = 
                    new Color(int.Parse(colors[0]) / 255f, int.Parse(colors[1]) / 255f, int.Parse(colors[2]) / 255f, 1f);
            }
        }
        
        public static void SaveColorSetToPalFile(ColorSet colorSet, string selectedDestinationPath)
        {
            var contents = new string[3 + colorSet.colors.Length];
            contents[0] = "JASC-PAL";
            contents[1] = "0100";
            contents[2] = $"{colorSet.colors.Length}";
                    
            for (var i = 0; i < colorSet.colors.Length; i++)
            {
                var color = colorSet.colors[i];
                contents[i + 3] = $"{(int)(color.r * 255f)} {(int)(color.g * 255f)} {(int)(color.b * 255f)}";
            }
            
            File.WriteAllLines(selectedDestinationPath, contents);
        }
    }
}