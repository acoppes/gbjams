using UnityEngine;

namespace GBJAM14.Data
{
    public class Room : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            UnityEditor.Handles.Label(transform.position, gameObject.name);
            #endif
        }
    }
}