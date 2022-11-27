using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace Beatemup.Ecs
{
    public static class HitBoxUtils
    {
        public static HitBox GetHitBox(this HitboxAsset hitBoxAsset, PositionComponent position, LookingDirection lookingDirection)
        {
            var offset = hitBoxAsset.offset;
                    
            if (lookingDirection.value.x < 0)
            {
                offset.x *= -1;
            }
                    
            return new HitBox
            {
                size = hitBoxAsset.size,
                position = new Vector2(position.value.x, position.value.y),
                offset = offset + new Vector2(0, position.value.z), 
                depth = hitBoxAsset.depth
            };
        }
    }
}