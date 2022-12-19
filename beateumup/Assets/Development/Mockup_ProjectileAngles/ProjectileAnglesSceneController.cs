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

        [Range(-1, 1)]
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

            // x = r * Mathf.Cos(theeta * Mathf.Deg2Rad) * Mathf.Sin(phi * Mathf.Deg2Rad);
            // y = r * Mathf.Sin(theeta * Mathf.Deg2Rad) * Mathf.Sin(phi * Mathf.Deg2Rad);
            // z = r * Mathf.Cos(phi * Mathf.Deg2Rad);
            
            var direction3d = new Vector3(x, y, z).normalized;
            var direction2d = new Vector2(x, y + z * 0.75f);

            var p0 = new Vector2(objectModel.transform.position.x, objectModel.model.transform.localPosition.y + objectModel.transform.position.y * 0.75f);
            var p1 = p0 + direction2d;

            var angle = Vector2.SignedAngle(Vector2.right, p1 - p0);

            D.raw(new Shape.Sphere(p0, 0.1f), Color.green);
            D.raw(new Shape.Sphere(p1, 0.1f), Color.green);

            D.raw(new Shape.Arrow(objectModel.transform.position, direction3d), Color.cyan);
            D.raw(new Shape.Arrow(objectModel.transform.position + new Vector3(0.2f, 0, 0), direction2d), Color.green);

            var t = objectModel.model.transform;

            t.localEulerAngles = new Vector3(0, 0, angle);
            t.localScale = new Vector3(direction2d.magnitude, 1, 1);
            
            direction3d = new Vector3(x, 0, z).normalized;
            direction2d = new Vector2(x, y * 0.1f + z * 0.75f);

            p0 = new Vector2(objectModel.transform.position.x, 0 + objectModel.transform.position.y * 0.75f);
            p1 = p0 + direction2d;

            angle = Vector2.SignedAngle(Vector2.right, p1 - p0);

            t = objectModel.shadow.transform;

            t.localEulerAngles = new Vector3(0, 0, angle);
            t.localScale = new Vector3(Mathf.Clamp(direction2d.magnitude, 0.1f, 1.0f), t.localScale.y, t.localScale.z);
        }
    }
}
