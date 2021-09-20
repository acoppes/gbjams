using GBJAM.Commons;
using UnityEngine;

namespace GBJAM9
{
    public class FreeUnitMovement : MonoBehaviour
    {
        [SerializeField]
        protected Transform transform;

        [SerializeField]
        protected GameboyButtonKeyMapAsset gameboyKeyMap;

        [SerializeField]
        protected float speed;

        [SerializeField]
        protected Vector2 perspective = new Vector2(1.0f, 0.75f);

        [SerializeField]
        protected Animator animator;

        [SerializeField]
        protected SpriteRenderer model;

        private int walkingStateHash = Animator.StringToHash("walking");
    
        // Update is called once per frame
        private void Update()
        {
            var myPosition = transform.localPosition;
            var velocity = gameboyKeyMap.direction * speed * Time.deltaTime;

            // TODO: vertical movement perspective....
        
            myPosition.x += velocity.x * perspective.x;
            myPosition.y += velocity.y * perspective.y;

            transform.localPosition = myPosition;

            if (animator != null)
            {
                animator.SetBool(walkingStateHash, velocity.SqrMagnitude() > 0);
            }

            if (model != null && Mathf.Abs(velocity.x) > 0)
            {
                model.flipX = velocity.x < 0;
            }
        }
    }
}
