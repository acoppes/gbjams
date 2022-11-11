using Beatemup.Ecs;
using UnityEditor;

namespace Utils.Editor
{
    public static class RefactorMethods {
        
        [MenuItem("Refactor/Convert animations to hit list")]
        public static void RefactorMethod1()
        {
            RefactorTools.RefactorAsset(delegate(AnimationsAsset asset)
            {
                var modified = false;
                
                foreach (var animationDefinition in asset.animations)
                {
                    foreach (var frame in animationDefinition.frames)
                    {
                        frame.hitBoxes.Clear();
                        if (frame.hitbox != null)
                        {
                            frame.hitBoxes.Add(frame.hitbox);
                        }
                        modified = true;
                    }
                }
                
                return modified;
            });
        }
    
    }
}