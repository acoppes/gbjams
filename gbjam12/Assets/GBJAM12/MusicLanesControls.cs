using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace GBJAM12
{
    public class MusicLanesControls : MonoBehaviour
    {
        // public PlayerInput playerInput;

        [FormerlySerializedAs("musicLaneConfiguration")] public GameConfiguration gameConfiguration;
        
        public List<InputActionReference> laneActions;

        public List<MusicLane> lanes;

        private void Update()
        {
            for (var i = 0; i < lanes.Count; i++)
            {
                var lane = lanes[i];
                var laneAction = laneActions[i];

                if (laneAction.action.WasPressedThisFrame())
                {
                    lane.StorePressedInTicks();
                }

                if (laneAction.action.IsPressed())
                {
                    // if (!lane.pressed)
                    // {
                    //     lane.StorePressedInTicks();
                    // }
                    lane.pressedBuffer = gameConfiguration.pressedTimeBuffer;
                    lane.pressed = true;
                }
                else
                {
                    lane.pressedBuffer -= Time.deltaTime;
                    lane.pressed = lane.pressedBuffer > 0;
                }
            }
        }
    }
}