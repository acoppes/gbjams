using UnityEngine;

namespace Beatemup.Ecs
{
    [CreateAssetMenu(menuName = "Tools/Create Hitbox", fileName = "HitboxAsset", order = 0)]
    public class HitboxAsset : ScriptableObject
    {
        public Vector3 offset;
        public Vector3 size;
    }
}