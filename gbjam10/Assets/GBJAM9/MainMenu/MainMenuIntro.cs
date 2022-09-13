using System;
using System.Collections.Generic;
using UnityEngine;

namespace GBJAM7.Scripts.MainMenu
{
    public class MainMenuIntro : MonoBehaviour
    {
        public Animator animator;
        
        [NonSerialized]
        public bool completed;
        
        public SpriteRenderer spriteRenderer;

        private int currentSprite;

        public bool visible = true;

        public List<Sprite> sprites;

        public bool Next()
        {
            if (currentSprite >= sprites.Count)
            {
                completed = true;
                animator.SetTrigger("Complete");
                return false;
            }
            
            animator.SetTrigger("Next");
            return true;
        }

        public void OnNextCompleted()
        {
            // completed = true;
            currentSprite++;
            // spriteRenderer.sprite = sprites[currentSprite++];
            Next();
        }
        
        
        public void OnCompleted()
        {
            completed = true;
            // Next();
        }
        
        private void LateUpdate()
        {
            animator.SetBool("visible", visible);
            
            if (currentSprite < sprites.Count)
            {
                spriteRenderer.sprite = sprites[currentSprite];
            }
        }
    }
}
