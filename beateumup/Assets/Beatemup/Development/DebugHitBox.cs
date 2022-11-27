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

            transform.position = hitBox.position;
            
            spriteRenderer.transform.localPosition = hitBox.offset;
            spriteRenderer.transform.localScale = new Vector3(hitBox.size.x, hitBox.size.y, 1);

            depthSpriteRenderer.transform.localPosition = new Vector3(hitBox.offset.x, 0, 0);
            depthSpriteRenderer.transform.localScale = new Vector3(hitBox.size.x, hitBox.depth * 2.0f, 1);

            spriteRenderer.color = typeColors[type];
        }
    }
}