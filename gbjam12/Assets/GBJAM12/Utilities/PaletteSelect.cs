using System;
using System.Collections.Generic;
using Game.DataAssets;
using Game.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace GBJAM12.Utilities
{
    public class PaletteSelect : MonoBehaviour
    {
        public static int currentPalette = 0;
        
        public GraphicPalette graphicPalette;

        public List<ColorSet> palettes;
        
        public void Update()
        {
            var keys = new KeyControl[]
            {
                Keyboard.current.digit1Key,
                Keyboard.current.digit2Key,
                Keyboard.current.digit3Key,
                Keyboard.current.digit4Key,
                Keyboard.current.digit5Key,
                Keyboard.current.digit6Key,
                Keyboard.current.digit7Key,
                Keyboard.current.digit8Key
            };

            for (var i = 0; i < keys.Length; i++)
            {
                var keyControl = keys[i];
                if (keyControl.wasReleasedThisFrame && i < palettes.Count)
                {
                    currentPalette = i;
                    // graphicPalette.colorSet = palettes[i];
                }
            }
            
            graphicPalette.colorSet = palettes[currentPalette];
        }
    }
}