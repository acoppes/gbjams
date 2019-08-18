using UnityEngine;

namespace GBJAM7.Scripts
{
    public class GameControls : MonoBehaviour
    {
        public GridObjectMovement selectorMovement;

        public BoundsInt worldBounds;
        
        public Camera worldCamera;
        
        // TODO: scroll camera if moving outside world bounds

        public KeyCode leftKey;
        public KeyCode rigthKey;
        public KeyCode upKey;
        public KeyCode downKey;
        
        public void Update()
        {
            // TODO: controls state, like "if in selection mode, then allow movement"
            
            if (Input.GetKeyDown(leftKey))
            {
                selectorMovement.Move(new Vector2Int(-1, 0));
            }
            
            if (Input.GetKeyDown(rigthKey))
            {
                selectorMovement.Move(new Vector2Int(1, 0));
            }
            
            if (Input.GetKeyDown(upKey))
            {
                selectorMovement.Move(new Vector2Int(0, 1));
            }
            
            if (Input.GetKeyDown(downKey))
            {
                selectorMovement.Move(new Vector2Int(0, -1));
            }
        }
    }
}
