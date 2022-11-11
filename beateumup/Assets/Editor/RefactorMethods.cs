using System.IO;
using Beatemup.Ecs;
using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    public static class RefactorMethods {
        
        [MenuItem("Refactor/Create Animation Metadata")]
        public static void RefactorMethod1()
        {
            // RefactorTools.RefactorAsset(delegate(AnimationsAsset asset)
            // {
            //     // var modified = false;
            //     //
            //     // var path = AssetDatabase.GetAssetPath(asset);
            //     //
            //     // var newAssetPath = Path.Combine(Path.GetDirectoryName(path), 
            //     //     $"{Path.GetFileNameWithoutExtension(path)}Metadata.asset");
            //     //
            //     // // Debug.Log(newAssetPath);
            //     // var metadataAsset = ScriptableObject.CreateInstance<AnimationHitboxMetadata>();
            //     //
            //     // foreach (var animationDefinition in asset.animations)
            //     // {
            //     //     for (var i = 0; i < animationDefinition.frames.Count; i++)
            //     //     {
            //     //         var frame = animationDefinition.frames[i];
            //     //     
            //     //         if (frame.hitBoxes.Count > 0)
            //     //         {
            //     //             metadataAsset.frameMetadata.Add(new AnimationHitboxFrameMetadata()
            //     //             {
            //     //                 animation = animationDefinition.name,
            //     //                 frame = i,
            //     //                 hitBoxes = frame.hitBoxes
            //     //             });
            //     //         }
            //     //         // modified = true;
            //     //     }
            //     // }
            //     //
            //     // AssetDatabase.CreateAsset(metadataAsset, newAssetPath);
            //     //
            //     // return modified;
            // });
        }
    }
}