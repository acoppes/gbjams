using System;
using UnityEngine;

namespace GBJAM7.Scripts
{
    public class UnitSelector : MonoBehaviour
    {
        public Vector3Int movement = new Vector3Int(1, 1, 0);

        [NonSerialized]
        public Vector3 position;

        public float moveSpeed = 1.0f;
        
        public Transform spriteTransform;

        public void Start()
        {
            position = transform.position;
            
            transform.position = Vector3.zero;
            spriteTransform.transform.position = position;
        }
        
        public void Move(Vector2Int direction)
        {
            position += new Vector3(movement.x * direction.x, movement.y * direction.y, 0);
        }

        public void LateUpdate()
        {
            spriteTransform.transform.position = Vector3.Lerp(spriteTransform.transform.position, position,
                moveSpeed * Time.deltaTime);
        }
    }
}
