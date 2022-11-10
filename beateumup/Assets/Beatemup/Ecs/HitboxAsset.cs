using UnityEngine;

namespace Beatemup.Ecs
{
    [CreateAssetMenu(menuName = "Create HitboxAsset", fileName = "HitboxAsset", order = 0)]
    public class HitboxAsset : ScriptableObject
    {
        public Vector2 offset;
        public Vector2 size;
        // public float depth;
    }
}