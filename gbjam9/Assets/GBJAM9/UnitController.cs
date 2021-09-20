using UnityEngine;

namespace GBJAM9
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField]
        protected Transform transform;

        [SerializeField]
        protected UnitInput unitInput;

        [SerializeField]
        protected float speed;

        [SerializeField]
        protected Vector2 perspective = new Vector2(1.0f, 0.75f);

        [SerializeField]
        protected UnitModel unitModel;
    
        // Update is called once per frame
        private void Update()
        {
            var myPosition = transform.localPosition;
            var velocity = unitInput.movementDirection * speed * Time.deltaTime;

            myPosition.x += velocity.x * perspective.x;
            myPosition.y += velocity.y * perspective.y;

            transform.localPosition = myPosition;

            unitModel.velocity = velocity;
        }
    }
}
