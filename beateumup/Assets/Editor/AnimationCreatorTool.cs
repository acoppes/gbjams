using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    public static class AnimationCreatorTool
    {
        private class KeyFrame
        {
            public Sprite sprite;
            public int frame;
        }
        
        private class Animation
        {
            public List<KeyFrame> keyframes = new List<KeyFrame>();
        }

        private const float DefaultFrameRate = 15.0f;
        
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
            var characterName = Path.GetFileNameWithoutExtension(path);

            var sprites = AssetDatabaseExt.FindAssets<Sprite>(new []
            {
                path
            });

            var animations = new Dictionary<string, Animation>();

            foreach (var sprite in sprites)
            {
                var spriteParts = sprite.name.Split("_");
                var animationName = spriteParts[0];
                var frame = int.Parse(spriteParts[1]);
                
                // var animationName = sprite.name.Substring(0, 
                //     sprite.name.IndexOf("_", StringComparison.OrdinalIgnoreCase));

                if (!animations.ContainsKey(animationName))
                {
                    animations[animationName] = new Animation();
                }

                var animation = animations[animationName];
                animation.keyframes.Add(new KeyFrame()
                {
                    sprite = sprite,
                    frame = frame
                });
            }

            foreach (var animationName in animations.Keys)
            {
                var animation = animations[animationName];
                
                var clip = new AnimationClip
                {
                    frameRate = DefaultFrameRate,
                    name = animationName
                };

                var spriteBinding = new EditorCurveBinding
                {
                    type = typeof(SpriteRenderer),
                    path = "Model",
                    propertyName = "m_Sprite"
                };

                // var frameDuration = 1.0f / fps;

                var referenceKeyFrames = new ObjectReferenceKeyframe[animation.keyframes.Count];
                
                for (var i = 0; i < animation.keyframes.Count; i++)
                {
                    referenceKeyFrames[i] = new ObjectReferenceKeyframe
                    {
                        time = animation.keyframes[i].frame / clip.frameRate,
                        value = animation.keyframes[i].sprite
                    };
                }
                
                AnimationUtility.SetAnimationClipSettings(clip, new AnimationClipSettings()
                {
                    loopTime = true
                });

                // // repeat last frame
                // if (frameTime > 1)
                // {
                //     var i = animation.keyframes.Count - 1;
                //     referenceKeyFrames[i + 1] = new ObjectReferenceKeyframe
                //     {
                //         time = ((animation.keyframes[i].frame + 1) * frameTime / fps) - frameDuration,
                //         value = animation.keyframes[i].sprite
                //     };
                // }
                
                AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, referenceKeyFrames);

                if (!AssetDatabase.IsValidFolder($"Assets/Animations/{characterName}"))
                {
                    AssetDatabase.CreateFolder("Assets/Animations", $"{characterName}");
                }

                var fileName = $"Assets/Animations/{characterName}/{animationName}.anim";
                
                var previousAnimationClip = AssetDatabase.LoadMainAssetAtPath(fileName) as AnimationClip;

                if (previousAnimationClip != null)
                {
                    previousAnimationClip.wrapMode = WrapMode.Loop;
                    EditorUtility.CopySerialized(clip, previousAnimationClip);
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    AssetDatabase.CreateAsset(clip, fileName);
                }
            }
            

        }
    }
}