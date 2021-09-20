using UnityEngine;

namespace GBJAM9
{
    public class KunaiController : MonoBehaviour
    {
        [SerializeField]
        protected UnitMovement movement;

        [SerializeField]
        protected UnitModel model;
        
        public void Fire(Vector3 position, Vector2 direction)
        {
            transform.position = position;
            movement.lookingDirection = direction;
        }

        private void Update()
        {
            movement.Move();
            model.lookingDirection = movement.velocity;
        }
    }
}