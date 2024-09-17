using Game.DataAssets;
using UnityEditor;
using UnityEditor.AssetImporters;
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
                    CopyPalFileContentsToColorSet(colorSet, selectedFile);
                    
                    EditorUtility.SetDirty(colorSet);
                }
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
            colorSet.colors[i] = new Color(int.Parse(colors[0]) / 255f, int.Parse(colors[1]) / 255f, int.Parse(colors[2]) / 255f, 1f);
        }
    }
    
    [ScriptedImporter(1, "pal")]
    public class ColorSetPaletteImporter : ScriptedImporter
    {
        // public float m_Scale = 1;

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var colorSet = ScriptableObject.CreateInstance<ColorSet>();
            CopyPalFileContentsToColorSet(colorSet, ctx.assetPath);

            // 'cube' is a GameObject and will be automatically converted into a prefab
            // (Only the 'Main Asset' is eligible to become a Prefab.)
            ctx.AddObjectToAsset("main obj", colorSet);
            ctx.SetMainObject(colorSet);
        }
    }
}
