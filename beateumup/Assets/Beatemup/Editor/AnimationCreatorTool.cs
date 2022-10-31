using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Beatemup.Editor
{
    public class AnimationCreatorTool
    {
        [UnityEditor.MenuItem("Assets/Create animation")]
        public static void CreateAnimationFromSelection()
        {
            var clip = new AnimationClip();

            var spriteBinding = new EditorCurveBinding();
            spriteBinding.type = typeof(SpriteRenderer);
            spriteBinding.path = "Model";
            spriteBinding.propertyName = "m_Sprite";

            var sprites = Selection.objects.OfType<Sprite>().ToList();

            var referenceKeyFrames = new ObjectReferenceKeyframe[sprites.Count];

            for (int i = 0; i < sprites.Count; i++)
            {
                referenceKeyFrames[i] = new ObjectReferenceKeyframe
                {
                    time = i,
                    value = sprites[i]
                };
            }
            
            AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, referenceKeyFrames);

            AssetDatabase.CreateAsset(clip, "Assets/Test.anim");
        }
    }
}