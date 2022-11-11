using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beatemup.Ecs
{
    [Serializable]
    public class AnimationHitboxFrameMetadata
    {
        public string animation;
        public int frame;
        
        public List<HitboxAsset> hitBoxes = new();
        
        // public List<HitboxAsset> hurtBoxes = new();
    }
    
    [CreateAssetMenu(menuName = "Tools/Create Animation Metadata", fileName = "AnimationMetadata", order = 0)]
    public class AnimationHitboxMetadata : ScriptableObject
    {
        public List<AnimationHitboxFrameMetadata> frameMetadata = new ();

        public AnimationHitboxFrameMetadata GetFrameMetadata(string animation, int frame)
        {
            return frameMetadata
                .FirstOrDefault(f => f.animation.Equals(animation, StringComparison.OrdinalIgnoreCase) && f.frame == frame);
        }
    }
}