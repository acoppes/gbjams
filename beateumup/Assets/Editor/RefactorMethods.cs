using System.Collections.Generic;
using System.IO;
using System.Linq;
using Beatemup.Definitions;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
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

        [MenuItem("Refactor/Switch to UnitInstanceParameter")]
        public static void RefactorMethod1()
        {
            RefactorTools.RefactorMonoBehaviour<UnitInstanceParameter>(true, delegate(GameObject o)
            {
                var modified = false;
                
                var unitInstanceParameter = o.GetComponent<UnitInstanceParameter>();
                var nameInstanceParameter = o.GetComponent<NameInstanceParameter>();
                
                if (nameInstanceParameter != null)
                {
                    unitInstanceParameter.entityName = nameInstanceParameter.entityName;
                    unitInstanceParameter.singleton = nameInstanceParameter.singleton;

                    Object.DestroyImmediate(nameInstanceParameter);
                    modified = true;
                }
                
                var playerTeamInstanceParameter = o.GetComponent<PlayerTeamInstanceParameter>();
                
                if (playerTeamInstanceParameter != null)
                {
                    unitInstanceParameter.overridePlayer = true;
                    unitInstanceParameter.team = playerTeamInstanceParameter.team;

                    Object.DestroyImmediate(playerTeamInstanceParameter);
                    modified = true;
                }
                
                return modified;
            });
            
            // RefactorTools.RefactorAsset(delegate(HitboxAsset asset)
            // {
            //     // var modified = false;
            //
            //     // asset.offset = new Vector3(asset.offset.x, asset.offset.y, asset.offset.z);
            //     asset.size = new Vector3(asset.size.x, asset.size.y, asset.depth);
            //
            //     return true;
            // });
        }
    }
}