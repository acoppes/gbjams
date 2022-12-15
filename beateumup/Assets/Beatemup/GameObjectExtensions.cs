using UnityEngine;

namespace Beatemup
{
    public static class GameObjectExtensions
    {
        public static bool IsSafeToModifyName(this GameObject gameObject)
        {
#if UNITY_2021_1_OR_NEWER
            if ( UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject) != null)
                return false;
#elif UNITY_2019_1_OR_NEWER
            if (UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject) != null)
                return false;
#endif

            if (!gameObject.scene.IsValid())
            {
                return false;
            }

            return true;
        }
    }
}