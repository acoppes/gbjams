using System;
using System.Collections.Generic;
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
        public List<AnimationHitboxFrameMetadata> frameMetadata = new List<AnimationHitboxFrameMetadata>();
    }
}