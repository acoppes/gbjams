using UnityEngine;
using UnityEngine.Events;

namespace GBJAM14.Scenes
{
    public class OnAwakeEvent : MonoBehaviour
    {
        public UnityEvent unityEvent;
        
        private void Awake()
        {
            unityEvent.Invoke();    
        }
    }
}