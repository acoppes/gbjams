using UnityEngine;

namespace Beatemup.Ecs
{
    [CreateAssetMenu(menuName = "Tools/Create Hitbox", fileName = "HitboxAsset", order = 0)]
    public class HitboxAsset : ScriptableObject
    {
        public Vector2 offset;
        public Vector2 size;
        public float depth = 0.5f;
    }
}