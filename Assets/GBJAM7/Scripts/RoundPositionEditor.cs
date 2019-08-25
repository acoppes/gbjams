using UnityEngine;

namespace GBJAM7.Scripts
{
    [ExecuteInEditMode]
    public class RoundPositionEditor : MonoBehaviour
    {
        private void Update()
        {
            var p = transform.position;
            transform.position = new Vector3(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y), Mathf.RoundToInt(p.z));
        }
    }
}