using UnityEngine;

namespace GBJAM7.Scripts
{
    [ExecuteInEditMode]
    public class TweenToReferencePosition : MonoBehaviour
    {
        public float speed;
        public Transform target;
        public Transform reference;
        
        public void LateUpdate()
        {
            if (Application.isPlaying)
            {
                target.position = Vector3.Lerp(target.position, reference.position,
                    speed * Time.deltaTime);
            }
            else
            {
                target.position = reference.position;
            }
        }
    }
}