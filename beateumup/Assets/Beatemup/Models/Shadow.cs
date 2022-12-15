using UnityEngine;

namespace Beatemup.Models
{
    public class Shadow : MonoBehaviour
    {
        public SpriteRenderer source;
        public SpriteRenderer target;

        private void LateUpdate()
        {
            target.sprite = source.sprite;
        }
    }
}
