using UnityEngine;

namespace GBJAM.Commons
{
    [ExecuteInEditMode]
    public class RoundPositionEditor : MonoBehaviour
    {
        public bool executeWhilePlaying = false;
        
        private void Update()
        {
            if (!executeWhilePlaying && Application.isPlaying)
                return;
            var p = transform.position;
            transform.position = new Vector3(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y), Mathf.RoundToInt(p.z));
        }
    }
}