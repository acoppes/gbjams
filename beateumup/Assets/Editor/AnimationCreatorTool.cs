﻿using System;
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

        // private const float DefaultFrameRate = 15f;

        public static List<int> GetFramesFromRanges(string frameRange)
        {
            var frames = new List<int>();
            var separations = frameRange.Split("_");

            foreach (var separation in separations)
            {
                var ranges = separation.Split("-");
                foreach (var range in ranges)
                {
                    frames.Add(int.Parse(range));
                }
            }
            
            return frames;
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
                var spriteParts = sprite.name.Split("_", 2);
                var animationName = spriteParts[0];
                var frameString = spriteParts[1];

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
                    s.name.Equals($"{animationName}fx_{frameString}", StringComparison.OrdinalIgnoreCase));

                var frames = GetFramesFromRanges(frameString);

                foreach (var frame in frames)
                {
                    animation.keyframes.Add(new KeyFrame
                    {
                        sprite = sprite,
                        fxSprite = fxSprite,
                        frame = frame
                    });
                }
                
                animation.hasFx = animation.hasFx || fxSprite;
            }

            foreach (var animationName in animations.Keys)
            {
                var animation = animations[animationName];
                animation.keyframes.Sort((a, b) => a.frame - b.frame);
            }

            var animationsAsset = ScriptableObject.CreateInstance<AnimationsAsset>();
            animationsAsset.name = characterName;

            foreach (var animationName in animations.Keys)
            {
                var animation = animations[animationName];

                var animationDefinition = new AnimationDefinition
                {
                    name = animationName,
                    // fps = DefaultFrameRate
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