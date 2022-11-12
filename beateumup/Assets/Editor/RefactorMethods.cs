using Beatemup.Ecs;
using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    public static class RefactorMethods {
        
        [MenuItem("Refactor/Switch to sprite metadata")]
        public static void RefactorMethod1()
        {
            RefactorTools.RefactorAsset(delegate(SpritesMetadata asset)
            {
                // var modified = false;
                
                var path = AssetDatabase.GetAssetPath(asset);

                var animationFile = path.Replace("Metadata", "");
                
                Debug.Log($"{animationFile}");
                
                var animationsAsset = AssetDatabase.LoadAssetAtPath<AnimationsAsset>(animationFile);

                foreach (var hitboxMetadata in asset.frameMetadata)
                {
                    var animationIndex = animationsAsset.FindByName(hitboxMetadata.animation);
                    var animation = animationsAsset.animations[animationIndex];

                    hitboxMetadata.sprite = animation.frames[hitboxMetadata.frame].sprite;
                }
                
                return true;
            });
        }
    }
}