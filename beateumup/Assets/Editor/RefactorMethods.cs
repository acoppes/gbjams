using System.Collections.Generic;
using System.IO;
using System.Linq;
using Beatemup.Definitions;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Gameplay.Controllers;
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

        [MenuItem("Refactor/Move controller to unit definition")]
        public static void RefactorMethod1()
        {
            RefactorTools.RefactorMonoBehaviour<PrefabEntityDefinition>(true, delegate(GameObject o)
            {
                var modified = false;
                
                var unitDefinition = o.GetComponent<UnitDefinition>();
                var controllerDefinition = o.GetComponent<ControllerDefinition>();
                
                if (controllerDefinition != null && unitDefinition == null)
                {
                    unitDefinition = o.AddComponent<UnitDefinition>();
                }
                
                if (controllerDefinition != null)
                {
                    unitDefinition.hasController = true;
                    unitDefinition.controllerObject = controllerDefinition.controllerObject;
                    
                    Object.DestroyImmediate(controllerDefinition);
                    
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