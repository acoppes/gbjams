using System;
using Beatemup.Models;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;
using Vertx.Debugging;

namespace Development
{
    [ExecuteAlways]
    public class ProjectileAnglesSceneController : MonoBehaviour
    {
        [FormerlySerializedAs("model")] 
        public Model objectModel;

        // public Vector3 direction3d;

        // [Range(-1, 1)]
        [ReadOnly]
        public float x, y, z;

        public float angleForward2;
        public float angleRight2;

        [Range(0, 360)]
        public float theeta;
        
        [Range(0, 180)]
        public float phi;
        
        // Update is called once per frame
        void Update()
        {
            const float r = 1.0f;

            x = r * Mathf.Cos(theeta * Mathf.Deg2Rad) * Mathf.Sin(phi * Mathf.Deg2Rad);
            y = r * Mathf.Sin(theeta * Mathf.Deg2Rad) * Mathf.Sin(phi * Mathf.Deg2Rad);
            z = r * Mathf.Cos(phi * Mathf.Deg2Rad);
            
            var direction3d = new Vector3(x, z, y);
            
            D.raw(new Shape.Arrow(objectModel.transform.position, direction3d), Color.cyan);
            D.raw(new Shape.Sphere(objectModel.transform.position, 0.05f), Color.green);
            
            // var r = direction3d.magnitude;
            
            //direction3d.x = 1.0f - (direction3d.y + direction3d.z);

            // theeta = Mathf.Atan(y / x) * Mathf.Rad2Deg;
            // phi = Mathf.Acos(z / r) * Mathf.Rad2Deg;
            
            var angleForward = Vector2.SignedAngle(Vector2.right, new Vector2(direction3d.x, direction3d.y + direction3d.z));
            var angleRight = Vector2.Angle(Vector2.right, new Vector2(Mathf.Abs(direction3d.x) + 0.25f, direction3d.z * 0.75f));
            
            angleForward2 = Vector2.SignedAngle(Vector2.right, new Vector2(direction3d.x, direction3d.y + direction3d.z));
            angleRight2 = Vector2.Angle(Vector2.right, new Vector2(Mathf.Abs(direction3d.z) + 0.25f, direction3d.x * 0.75f));
            
            // var angleY = Vector2.SignedAngle(Vector2.right, new Vector2(direction3d.x, direction3d.y));

            // var angle = Mathf.Atan2(direction3d.z, direction3d.x) * Mathf.Rad2Deg;
            var angleAxis = Quaternion.AngleAxis(angleForward, Vector3.forward);
            var angleRightAxis = Quaternion.AngleAxis(angleRight, Vector3.right);
            
            var angleAxis2 = Quaternion.AngleAxis(angleRight2, Vector3.right);
            var angleRightAxis2 = Quaternion.AngleAxis(angleForward2, Vector3.forward);
                    
            // var position2d = new Vector3(position3d.x, position3d.y + position3d.z * 0.75f);
            // var direction2d = new Vector3(direction.x, direction.y + direction.z * 0.75f);

            objectModel.model.transform.localEulerAngles = angleAxis.eulerAngles + angleRightAxis.eulerAngles;
            objectModel.shadow.transform.localEulerAngles = angleRightAxis2.eulerAngles + new Vector3(70, 0, 0);
        }
    }
}
