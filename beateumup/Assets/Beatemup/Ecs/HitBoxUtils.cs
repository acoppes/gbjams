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
                position = position.value,
                offset = offset
            };
        }
    }
}