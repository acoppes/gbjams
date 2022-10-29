using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Beatemup.Editor
{
    public class AutoloadLevelChunksAssetPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var dataAssets = AssetDatabase.FindAssets($"t:{typeof(LevelDataAsset)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<LevelDataAsset>).ToList();
            
            foreach (var dataAsset in dataAssets)
            {
                if (string.IsNullOrEmpty(dataAsset.chunksPrefabsPath))
                    continue;

                var shouldRegenerate = false;

                foreach (var asset in importedAssets)
                {
                    shouldRegenerate = asset.StartsWith(dataAsset.chunksPrefabsPath) &&
                                       asset.EndsWith(".prefab");
                }
                
                foreach (var asset in deletedAssets)
                {
                    shouldRegenerate = asset.StartsWith(dataAsset.chunksPrefabsPath) &&
                                       asset.EndsWith(".prefab");
                }

                if (shouldRegenerate)
                {
                    dataAsset.chunksList.Clear();

                    dataAsset.chunksList = AssetDatabase.FindAssets("t:GameObject", new[] {dataAsset.chunksPrefabsPath})
                        .Select(AssetDatabase.GUIDToAssetPath)
                        .Select(AssetDatabase.LoadAssetAtPath<GameObject>).ToList();

                    EditorUtility.SetDirty(dataAsset);
                }
                
                AssetDatabase.SaveAssetIfDirty(dataAsset);    
            }
        }
    }
}