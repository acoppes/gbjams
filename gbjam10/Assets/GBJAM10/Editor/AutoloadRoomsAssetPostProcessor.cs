using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GBJAM10.Editor
{
    public class AutoloadRoomsAssetPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var roomDataAssets = AssetDatabase.FindAssets("t:RoomDataAsset")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<RoomDataAsset>).ToList();
            
            foreach (var roomDataAsset in roomDataAssets)
            {
                if (string.IsNullOrEmpty(roomDataAsset.roomAssetsPath))
                    continue;

                var shouldRegenerate = false;

                foreach (var asset in importedAssets)
                {
                    shouldRegenerate = asset.StartsWith(roomDataAsset.roomAssetsPath) &&
                                       asset.EndsWith(".prefab");
                }
                
                foreach (var asset in deletedAssets)
                {
                    shouldRegenerate = asset.StartsWith(roomDataAsset.roomAssetsPath) &&
                                       asset.EndsWith(".prefab");
                }

                if (shouldRegenerate)
                {
                    roomDataAsset.roomPrefabs.Clear();

                    roomDataAsset.roomPrefabs = AssetDatabase.FindAssets("t:GameObject", new[] {roomDataAsset.roomAssetsPath})
                        .Select(AssetDatabase.GUIDToAssetPath)
                        .Select(AssetDatabase.LoadAssetAtPath<GameObject>).ToList();

                    EditorUtility.SetDirty(roomDataAsset);
                }
                
                AssetDatabase.SaveAssetIfDirty(roomDataAsset);    
            }
        }
    }
}