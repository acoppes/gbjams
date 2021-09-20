using UnityEngine;

namespace GBJAM9
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        protected Transform transform;

        [SerializeField]
        protected Transform cameraTransform;

        private void LateUpdate()
        {
            var p = cameraTransform.transform.position;
            p.x = transform.position.x;
            p.y = transform.position.y;
            cameraTransform.transform.position = p;

        }
    }
}