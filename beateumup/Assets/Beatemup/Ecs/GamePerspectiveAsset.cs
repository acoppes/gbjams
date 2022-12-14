using UnityEngine;

namespace Beatemup.Ecs
{
    [CreateAssetMenu(menuName = "Gemserk/GamePerspective")]
    public class GamePerspectiveAsset : ScriptableObject
    {
        public float gamePerspectiveY = 0.75f;
        
        public Vector3 ConvertToWorld(Vector3 v)
        {
            return new Vector3(v.x, v.z / gamePerspectiveY, v.y);
        }

        public Vector3 ConvertFromWorld(Vector3 v)
        {
            return new Vector3(v.x, v.z * gamePerspectiveY, v.y);
        }
    }
}