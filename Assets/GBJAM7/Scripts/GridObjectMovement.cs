using UnityEngine;

namespace GBJAM7.Scripts
{
    public class GridObjectMovement : MonoBehaviour
    {
        public Vector3Int movement = new Vector3Int(4, 4, 0);

        public void Move(Vector2Int direction)
        {
            transform.position += new Vector3(movement.x * direction.x, movement.y * direction.y, 0);
        }
    }
}
