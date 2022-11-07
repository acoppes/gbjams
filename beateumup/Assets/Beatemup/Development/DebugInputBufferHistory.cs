using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Beatemup.Development
{
    public class DebugInputBufferHistory : MonoBehaviour
    {
        public string playerName;

        public GameObject debugBufferPrefab;

        private DebugBuffer _debugBuffer;

        private World _world;

        public void Start()
        {
            _world = World.Instance;
        }
        private void Update()
        {
            var entity = _world.GetEntityByName(playerName);
            if (entity == Entity.NullEntity)
            {
                return;
            }

            var controlComponent = _world.GetComponent<ControlComponent>(entity);

            if (_debugBuffer == null)
            {
                var debugBufferInstance = GameObject.Instantiate(debugBufferPrefab, transform);
                _debugBuffer = debugBufferInstance.GetComponent<DebugBuffer>();
            }
            
            _debugBuffer.UpdateBuffer(controlComponent);
        }
    }
}
