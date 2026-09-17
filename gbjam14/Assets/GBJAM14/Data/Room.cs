using System;
using UnityEngine;

namespace GBJAM14.Data
{
    public class Room : MonoBehaviour
    {
        [NonSerialized]
        public GameObject confiner;

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            var drawingScope = new UnityEditor.Handles.DrawingScope();
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.Label(transform.position, gameObject.name);
            UnityEditor.Handles.color = drawingScope.originalColor;
            drawingScope.Dispose();
            #endif
        }
    }
}