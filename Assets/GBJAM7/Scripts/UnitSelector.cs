using UnityEngine;

namespace GBJAM7.Scripts
{
    public class UnitSelector : MonoBehaviour
    {
        public Vector3Int movement = new Vector3Int(1, 1, 0);

        public void Move(Vector2Int direction)
        {
            transform.position += new Vector3(movement.x * direction.x, movement.y * direction.y, 0);
        }
    }
}
