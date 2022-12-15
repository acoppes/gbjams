using System;
using Beatemup.Ecs;
using UnityEngine;

namespace Beatemup.Development
{
    public class DebugHitBox : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer depthSpriteRenderer;

        public Color[] typeColors = new Color[4];
        
        [NonSerialized]
        public DebugHitBoxSystem debugHitBoxSystem;

        private HitBox hitBox;

        [NonSerialized]
        public int type;

        [NonSerialized] 
        public GamePerspectiveAsset gamePerspective;

        // [NonSerialized]
        // public Vector2 gamePerspective;

        public void UpdateHitBox(HitBox hitBox)
        {
            this.hitBox = hitBox;
        }

        private void Update()
        {
            spriteRenderer.enabled = debugHitBoxSystem.debugHitBoxesEnabled;
            depthSpriteRenderer.enabled = debugHitBoxSystem.debugHitBoxesEnabled;
            
            if (!spriteRenderer.enabled)
            {
                return;
            }

            // transform.position = new Vector3(hitBox.position.x * gamePerspective.x, hitBox.position.y * gamePerspective.y, 0);
            var position = gamePerspective.ConvertFromWorld(hitBox.position);
            var offset = hitBox.offset;
            var size = hitBox.size;
            
            transform.position = position;
            
            spriteRenderer.transform.localPosition = new Vector3(offset.x, offset.y, 0);
            spriteRenderer.transform.localScale = new Vector3(size.x, size.y, 1);

            depthSpriteRenderer.transform.localPosition = new Vector3(offset.x, 0, 0);
            depthSpriteRenderer.transform.localScale = new Vector3(size.x, size.z, 1);

            spriteRenderer.color = typeColors[type];
        }
    }
}