using UnityEngine;

namespace GBJAM12
{
    public class MistakeJawUI : MonoBehaviour
    {
        public GameObject jawObject;

        public bool isActive;

        private void LateUpdate()
        {
            jawObject.SetActive(isActive);
        }
    }
}