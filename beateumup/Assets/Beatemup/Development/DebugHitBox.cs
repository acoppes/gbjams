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
            transform.position = new Vector3(hitBox.position.x, hitBox.position.y, 0);
            
            spriteRenderer.transform.localPosition = new Vector3(hitBox.offset.x, hitBox.offset.y, 0);
            spriteRenderer.transform.localScale = new Vector3(hitBox.size.x, hitBox.size.y, 1);

            depthSpriteRenderer.transform.localPosition = new Vector3(hitBox.offset.x, 0, 0);
            depthSpriteRenderer.transform.localScale = new Vector3(hitBox.size.x, hitBox.depth * 2.0f, 1);

            spriteRenderer.color = typeColors[type];
        }
    }
}