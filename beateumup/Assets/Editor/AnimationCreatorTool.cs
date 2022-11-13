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

        // private const float DefaultFrameRate = 15f;

        public static List<int> GetFramesFromRanges(string frameRange)
        {
            var frames = new List<int>();
            var separations = frameRange.Split("_");

            foreach (var separation in separations)
            {
                var ranges = separation.Split("-");

                if (ranges.Length == 2)
                {
                    var start = int.Parse(ranges[0]);
                    var end = int.Parse(ranges[1]);

                    for (var i = start; i <= end; i++)
                    {
                        frames.Add(i);
                    }
                } else if (ranges.Length == 1)
                {
                    frames.Add(int.Parse(ranges[0]));
                }
            }
            
            return frames;
        }

        [UnityEditor.MenuItem("Assets/Expand files in Folder")]
        public static void ExpandFiles()
        {
            var folderToExpand = EditorUtility.OpenFolderPanel("Select folder", Application.dataPath, "");

            if (!string.IsNullOrEmpty(folderToExpand))
            {
                var targetFolder = Path.Combine(folderToExpand, "Expanded");

                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }
                
                var files = Directory.GetFiles(folderToExpand, "*.png");
                foreach (var file in files)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    
                    var spriteParts = fileName.Split("_", 2);

                    if (spriteParts.Length < 2)
                    {
                        continue;
                    }
                    
                    var animationName = spriteParts[0];
                    var frameString = spriteParts[1];
                    
                    // Debug.Log($"{frameString}");

                    if (string.IsNullOrEmpty(animationName))
                    {
                        continue;
                    }
                    
                    if (string.IsNullOrEmpty(frameString))
                    {
                        continue;
                    }
                    
                    // Debug.Log($"Convert {fileName} to multiple files");

                    var frames = GetFramesFromRanges(frameString);

                    foreach (var frame in frames)
                    {
                        var target = Path.Combine(targetFolder, $"{animationName}_{frame:00}.png");
                        if (!File.Exists(target))
                        {
                            FileUtil.CopyFileOrDirectory(file, target);
                        }
                    }
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