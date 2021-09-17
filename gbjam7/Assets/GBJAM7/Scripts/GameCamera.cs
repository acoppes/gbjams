using System;
using UnityEngine;

namespace GBJAM7.Scripts
{
    [ExecuteInEditMode]
    public class GameCamera : MonoBehaviour
    {
        [NonSerialized]
        public Vector3 position;

//        private void Start()
//        {
//            position = transform.position;
//        }
        
        private void LateUpdate()
        {
            if (Application.isPlaying)
            {
                var p = transform.position;
                p.x = position.x;
                p.y = position.y;
                transform.position = p;
            }
            else
            {
                position = transform.position;
            }
        }
    }
}