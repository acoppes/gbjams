using Game.DataAssets;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace GBJAM12.Editor
{
    [ScriptedImporter(1, "pal")]
    public class ColorSetPaletteImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var colorSet = ScriptableObject.CreateInstance<ColorSet>();
            PalleteToColorMapImporter.CopyPalFileContentsToColorSet(colorSet, ctx.assetPath);
            // 'cube' is a GameObject and will be automatically converted into a prefab
            // (Only the 'Main Asset' is eligible to become a Prefab.)
            ctx.AddObjectToAsset("main obj", colorSet);
            ctx.SetMainObject(colorSet);
        }
    }
}