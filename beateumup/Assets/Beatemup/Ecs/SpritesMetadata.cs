using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beatemup.Ecs
{
    [Serializable]
    public class HitboxMetadata
    {
        public Sprite sprite;
        
        public string animation;
        public int frame;
        
        public List<HitboxAsset> hitBoxes = new();
        
        // public List<HitboxAsset> hurtBoxes = new();
    }
    
    [CreateAssetMenu(menuName = "Tools/Create Sprites Metadata", fileName = "SpritesMetadata", order = 0)]
    public class SpritesMetadata : ScriptableObject
    {
        public List<HitboxMetadata> frameMetadata = new ();

        public HitboxMetadata GetFrameMetadata(string animation, int frame)
        {
            return frameMetadata
                .FirstOrDefault(f => f.animation.Equals(animation, StringComparison.OrdinalIgnoreCase) && f.frame == frame);
        }
    }
}