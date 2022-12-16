using UnityEngine;
using Vertx.Debugging;

[ExecuteAlways]
public class DebugArrow : MonoBehaviour
{
    public Color color = Color.red;
    public Vector3 direction;
    
    // Update is called once per frame
    public void Update()
    {
        D.raw(new Shape.Arrow(transform.position, direction), color);
        D.raw(new Shape.Sphere(transform.position, 0.05f), Color.green);
    }
}
