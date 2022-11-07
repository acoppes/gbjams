using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class AnimationsAsset : ScriptableObject
    {
        public List<AnimationDefinition> animations = new();

        public int FindByName(string animationName)
        {
            for (var i = 0; i < animations.Count; i++)
            {
                var animation = animations[i];
                if (animationName.Equals(animation.name, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}