using System;
using System.Collections.Generic;
using System.IO;
using Beatemup.Ecs;
using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    public static class AnimationCreatorTool
    {
        private class KeyFrame
        {
            public Sprite sprite;
            public Sprite fxSprite;
            public int frame;
        }
        
        private class Animation
        {
            public List<KeyFrame> keyframes = new List<KeyFrame>();
            public bool hasFx;
        }

        private const float DefaultFrameRate = 12.5f;
        
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
                var frame = spriteParts[1];

                if (animationName.EndsWith("fx", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                    // animationName = animationName.Replace("fx", "");
                }
                
                // var animationName = sprite.name.Substring(0, 
                //     sprite.name.IndexOf("_", StringComparison.OrdinalIgnoreCase));

                if (!animations.ContainsKey(animationName))
                {
                    animations[animationName] = new Animation();
                }

                var animation = animations[animationName];

                var fxSprite = sprites.Find(s =>
                    s.name.Equals($"{animationName}fx_{frame}", StringComparison.OrdinalIgnoreCase));
                
                animation.keyframes.Add(new KeyFrame()
                {
                    sprite = sprite,
                    fxSprite = fxSprite,
                    frame = int.Parse(frame)
                });

                animation.hasFx = animation.hasFx || fxSprite;
            }

            foreach (var animationName in animations.Keys)
            {
                var animation = animations[animationName];
                
                var clip = new AnimationClip
                {
                    frameRate = DefaultFrameRate,
                    name = animationName
                };
                
                AnimationUtility.SetAnimationClipSettings(clip, new AnimationClipSettings()
                {
                    loopTime = true
                });

                var spriteBinding = new EditorCurveBinding
                {
                    type = typeof(SpriteRenderer),
                    path = "Model",
                    propertyName = "m_Sprite"
                };
                
                var spriteReferenceKeyFrames = new ObjectReferenceKeyframe[animation.keyframes.Count];
                
                for (var i = 0; i < animation.keyframes.Count; i++)
                {
                    spriteReferenceKeyFrames[i] = new ObjectReferenceKeyframe
                    {
                        time = animation.keyframes[i].frame / clip.frameRate,
                        value = animation.keyframes[i].sprite
                    };
                }
                
                AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, spriteReferenceKeyFrames);
                
                if (animation.hasFx)
                {
                    var spriteFxBinding = new EditorCurveBinding
                    {
                        type = typeof(SpriteRenderer),
                        path = "Effect",
                        propertyName = "m_Sprite"
                    };

                    var effectReferenceKeyFrames = new ObjectReferenceKeyframe[animation.keyframes.Count];

                    for (var i = 0; i < animation.keyframes.Count; i++)
                    {
                        effectReferenceKeyFrames[i] = new ObjectReferenceKeyframe
                        {
                            time = animation.keyframes[i].frame / clip.frameRate,
                            value = animation.keyframes[i].fxSprite
                        };
                    }

                    AnimationUtility.SetObjectReferenceCurve(clip, spriteFxBinding, effectReferenceKeyFrames);
                }
                
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
        
        [UnityEditor.MenuItem("Assets/Create Animation Asset from Folder")]
        public static void CreateAnimationAssetFromFolder()
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
                var frame = spriteParts[1];

                if (animationName.EndsWith("fx", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                    // animationName = animationName.Replace("fx", "");
                }
                
                // var animationName = sprite.name.Substring(0, 
                //     sprite.name.IndexOf("_", StringComparison.OrdinalIgnoreCase));

                if (!animations.ContainsKey(animationName))
                {
                    animations[animationName] = new Animation();
                }

                var animation = animations[animationName];

                var fxSprite = sprites.Find(s =>
                    s.name.Equals($"{animationName}fx_{frame}", StringComparison.OrdinalIgnoreCase));
                
                animation.keyframes.Add(new KeyFrame()
                {
                    sprite = sprite,
                    fxSprite = fxSprite,
                    frame = int.Parse(frame)
                });

                animation.hasFx = animation.hasFx || fxSprite;
            }

            var animationsAsset = ScriptableObject.CreateInstance<AnimationsAsset>();
            animationsAsset.name = characterName;

            foreach (var animationName in animations.Keys)
            {
                var animation = animations[animationName];

                var animationDefinition = new AnimationDefinition
                {
                    name = animationName,
                    fps = DefaultFrameRate
                };

                for (var i = 0; i < animation.keyframes.Count; i++)
                {
                    animationDefinition.frames.Add(new AnimationFrame()
                    {
                        // frame = animation.keyframes[i].frame,
                        sprite = animation.keyframes[i].sprite,
                        fxSprite = animation.keyframes[i].fxSprite
                    });
                }
                
                animationsAsset.animations.Add(animationDefinition);
            }
            
            if (!AssetDatabase.IsValidFolder($"Assets/Animations"))
            {
                AssetDatabase.CreateFolder("Assets", "Animations");
            }

            var fileName = $"Assets/Animations/{characterName}.asset";
                
            var previousAnimationAsset = AssetDatabase.LoadMainAssetAtPath(fileName) as AnimationsAsset;

            if (previousAnimationAsset != null)
            {
                EditorUtility.SetDirty(animationsAsset);
                EditorUtility.CopySerialized(animationsAsset, previousAnimationAsset);
                AssetDatabase.SaveAssets();
            }
            else
            {
                EditorUtility.SetDirty(animationsAsset);
                AssetDatabase.CreateAsset(animationsAsset, fileName);
            }
            
            EditorGUIUtility.PingObject(animationsAsset);
        }
    }
}