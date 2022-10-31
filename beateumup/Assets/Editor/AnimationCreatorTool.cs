using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    public static class AnimationCreatorTool
    {
        [UnityEditor.MenuItem("Assets/Create Animations from Folder")]
        public static void CreateAnimationsFromFolder()
        {
            var activeObject = Selection.activeObject;

            if (activeObject == null)
            {
                // show message?
                return;
            }

            var path = AssetDatabase.GetAssetPath(activeObject);

            var sprites = AssetDatabaseExt.FindAssets<Sprite>(new []
            {
                path
            });

            foreach (var sprite in sprites)
            {
                Debug.Log(sprite.name);
            }
            
            //
            //
            //
            // var clip = new AnimationClip();
            //
            // var spriteBinding = new EditorCurveBinding();
            // spriteBinding.type = typeof(SpriteRenderer);
            // spriteBinding.path = "Model";
            // spriteBinding.propertyName = "m_Sprite";
            //
            // var sprites = Selection.objects.OfType<Sprite>().ToList();
            //
            // var referenceKeyFrames = new ObjectReferenceKeyframe[sprites.Count];
            //
            // for (int i = 0; i < sprites.Count; i++)
            // {
            //     referenceKeyFrames[i] = new ObjectReferenceKeyframe
            //     {
            //         time = i,
            //         value = sprites[i]
            //     };
            // }
            //
            // AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, referenceKeyFrames);
            //
            // AssetDatabase.CreateAsset(clip, "Assets/Test.anim");
        }
    }
}