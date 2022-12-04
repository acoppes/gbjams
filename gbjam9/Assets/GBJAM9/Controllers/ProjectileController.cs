using GBJAM.Commons.Prefabs.Sfx;
using GBJAM9.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace GBJAM9.Controllers
{
    public class ProjectileController : MonoBehaviour
    {
        [FormerlySerializedAs("entityComponent")]
        [FormerlySerializedAs("unitComponent")] 
        [FormerlySerializedAs("unit")] 
        public Entity entity;

        [FormerlySerializedAs("sfx")] [SerializeField]
        protected SfxVariant fireSfx;

        public float startOffset = 0.4f;

        public void Fire(Vector3 position, Vector2 direction)
        {
            var offset = direction.normalized * startOffset;
            transform.position = position + new Vector3(offset.x, offset.y, 0);
            
            // entityComponent.movement.lookingDirection = direction;
            entity.movement.movingDirection = direction;
            entity.model.lookingDirection = direction;
            
            if (fireSfx != null)
            {
                fireSfx.Play();
            }
        }

        // private void Update()
        // {
        //     entity.model.lookingDirection = entity.movement.velocity;
        // }
    }
}