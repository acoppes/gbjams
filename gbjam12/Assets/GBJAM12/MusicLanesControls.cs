using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GBJAM12
{
    public class MusicLanesControls : MonoBehaviour
    {
        // public PlayerInput playerInput;

        public MusicLaneConfiguration musicLaneConfiguration;
        
        public List<InputActionReference> laneActions;

        public List<MusicLane> lanes;

        private void Update()
        {
            for (var i = 0; i < lanes.Count; i++)
            {
                var lane = lanes[i];
                var laneAction = laneActions[i];

                if (laneAction.action.IsPressed())
                {
                    lane.pressedBuffer = musicLaneConfiguration.pressedTimeBuffer;
                }
                else
                {
                    lane.pressedBuffer -= Time.deltaTime;
                }
            }
        }
    }
}