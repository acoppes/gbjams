using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Beatemup.Development
{
    public class DebugInputBufferHistory : MonoBehaviour
    {
        public string playerName;

        [SerializeField]
        private int maxHistory = 10;
        
        [SerializeField]
        private GameObject debugBufferPrefab;

        private DebugBuffer _debugBuffer;

        private World _world;

        public void Start()
        {
            _world = World.Instance;
        }
        private void Update()
        {
            // var entity = _world.GetEntityByName(playerName);

            var target = _world.GetFirstTarget(new TargetingUtils.RuntimeTargetingParameters()
            {
                name = playerName,
                playerAllianceType = TargetingUtils.PlayerAllianceType.Everything
            });

            if (target == null)
            {
                return;
            }
            
            if (target.entity == Entity.NullEntity)
            {
                return;
            }

            var controlComponent = _world.GetComponent<ControlComponent>(target.entity);

            if (controlComponent.buffer.Count == 0)
            {
                if (_debugBuffer != null)
                {
                    _debugBuffer.ConvertToHistory();
                }
                _debugBuffer = null;
                return;
            }

            if (_debugBuffer == null)
            {
                var debugBufferInstance = GameObject.Instantiate(debugBufferPrefab, transform);
                _debugBuffer = debugBufferInstance.GetComponent<DebugBuffer>();
            }
            
            _debugBuffer.UpdateBuffer(controlComponent);
            
            if (transform.childCount > maxHistory)
            {
                var firstChild = transform.GetChild(0);
                GameObject.DestroyImmediate(firstChild.gameObject);
            }
        }
    }
}
