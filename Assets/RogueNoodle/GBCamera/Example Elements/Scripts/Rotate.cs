using UnityEngine;
using System.Collections;

namespace RogueNoodle
{

namespace GBCamera
{

public class Rotate : MonoBehaviour {

	[Tooltip ("Axis of rotation - use 0 or 1 only")]
	public Vector3 _rotationAxis = new Vector3 (0,1,0);
	[Tooltip ("Speed of rotation in degrees per second")]
	public float _rotationSpeed = 90;
	
	private Transform _transform;
	
	void Start () {
	
		// cache the transform
		_transform = transform;
	}
	
	
	void Update ()
	{
		_transform.Rotate (_rotationAxis * _rotationSpeed * Time.deltaTime);
	}
}

}
}
