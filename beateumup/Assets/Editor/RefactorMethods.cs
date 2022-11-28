using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    public static class RefactorMethods {
        
        [MenuItem("Refactor/Reserialize All Assets")]
        public static void ReserializeAssets()
        {
            if (EditorUtility.DisplayDialog("Reserialize", "Force reserialize all assets?", "Ok", "Cancel"))
            {
                AssetDatabase.ForceReserializeAssets();
            }
        }
        
        [MenuItem("Refactor/Reserialize Selected Assets")]
        public static void ReserializeSelectedAssets()
        {
            var selectedObjects = Selection.objects;
            if (selectedObjects.Length == 0)
            {
                return;
            }

            var assetPaths = selectedObjects
                .Where(obj => AssetDatabase.GetAssetPath(obj) != null)
                .Select(obj => AssetDatabase.GetAssetPath(obj));

            var allPaths = new List<string>();

            foreach (var assetPath in assetPaths)
            {
                allPaths.Add(assetPath);
                
                if (Directory.Exists(assetPath))
                {
                    var objectsInside = AssetDatabase.FindAssets("t:Object", new []{assetPath});
                    var paths = objectsInside.Select(obj => AssetDatabase.GUIDToAssetPath(obj));

                    allPaths.AddRange(paths);
                }
            }

            if (EditorUtility.DisplayDialog("Reserialize", "Force reserialize selected assets?", "Ok", "Cancel"))
            {
                AssetDatabase.ForceReserializeAssets(allPaths);
                // allPaths.ForEach(p => Debug.Log(p));
            }
        }

        [MenuItem("Refactor/Switch to sprite metadata")]
        public static void RefactorMethod1()
        {
            // RefactorTools.RefactorAsset(delegate(SpritesMetadata asset)
            // {
            //     // var modified = false;
            //     
            //     var path = AssetDatabase.GetAssetPath(asset);
            //
            //     var animationFile = path.Replace("Metadata", "");
            //     
            //     Debug.Log($"{animationFile}");
            //     
            //     var animationsAsset = AssetDatabase.LoadAssetAtPath<AnimationsAsset>(animationFile);
            //
            //     foreach (var hitboxMetadata in asset.frameMetadata)
            //     {
            //         var animationIndex = animationsAsset.FindByName(hitboxMetadata.animation);
            //         var animation = animationsAsset.animations[animationIndex];
            //
            //         hitboxMetadata.sprite = animation.frames[hitboxMetadata.frame].sprite;
            //     }
            //     
            //     return true;
            // });
        }
    }
}