using UnityEngine;
using UnityEngine.Serialization;

namespace GBJAM9
{
    public class CameraFollow : MonoBehaviour
    {
        [FormerlySerializedAs("transform")] 
        public Transform followTransform;
        public Transform cameraTransform;

        private void LateUpdate()
        {
            if (cameraTransform == null || followTransform == null) 
                return;
            
            var p = cameraTransform.transform.position;
            p.x = followTransform.position.x;
            p.y = followTransform.position.y;
            cameraTransform.transform.position = p;

        }
    }
}