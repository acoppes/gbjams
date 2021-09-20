using System;
using GBJAM.Commons;
using UnityEngine;

namespace GBJAM9
{
    public class UnitInput : MonoBehaviour
    {
        private class ButtonDoubleTapDetection
        {
            public float delay;

            public bool isDoubleTap;

            private float notPressedTime;
            private bool wasPressed;
            
            public void Track(bool pressed, float dt)
            {
                isDoubleTap = false;
                
                if (pressed)
                {
                    if (!wasPressed)
                    {
                        isDoubleTap = notPressedTime < delay;
                    }
                }
                else
                {
                    if (wasPressed)
                    {
                        notPressedTime = 0;
                    }

                    notPressedTime += dt;
                }

                wasPressed = pressed;
            }
        }
        
        [SerializeField]
        protected GameboyButtonKeyMapAsset gameboyKeyMap;

        [NonSerialized]
        public Vector2 movementDirection;

        [NonSerialized]
        public bool fireKunai;

        [NonSerialized]
        public bool dash;
        
        [NonSerialized]
        public Vector2 dashDirection;
        
        public float doubleTapDelay = 1;

        private ButtonDoubleTapDetection[] doubleTapDetections;
        
        private void Awake()
        {
            doubleTapDetections = new ButtonDoubleTapDetection[4];
            for (var i = 0; i < doubleTapDetections.Length; i++)
            {
                doubleTapDetections[i] = new ButtonDoubleTapDetection() {delay = doubleTapDelay};
            }
        }

        private void Update()
        {
            dash = false;
            dashDirection = Vector2.zero;
            
            movementDirection = gameboyKeyMap.direction;
            fireKunai = gameboyKeyMap.button2Pressed;

            var pressed = new bool[]
            {
                gameboyKeyMap.rightPressed,
                gameboyKeyMap.leftPressed,
                gameboyKeyMap.downPressed,
                gameboyKeyMap.upPressed
            };

            var directions = new Vector2[]
            {
                new Vector2(1, 0),
                new Vector2(-1, 0),
                new Vector2(0, -1),
                new Vector2(0, 1),
            };
            
            for (var i = 0; i < doubleTapDetections.Length; i++)
            {
                doubleTapDetections[i].Track(pressed[i], Time.deltaTime);
                if (doubleTapDetections[i].isDoubleTap)
                {
                    dash = true;
                    dashDirection += directions[i];
                }
            }

        }
    }
}