using System;
using System.Collections;
using System.Collections.Generic;
using Gemserk.Utilities.UI;
using UnityEngine;

namespace GBJAM14.UI
{
    public class UIPauseMenu : MonoBehaviour
    {
        public UIWindow window;
        public UIOptions uiOptions;

        private int canOpenDisabledFrames;

        public void FixedUpdate()
        {
            canOpenDisabledFrames--;
        }

        public void OpenPauseMenu()
        {
            if (window.IsOpen())
            {
                return;
            }

            if (canOpenDisabledFrames > 0)
            {
                return;
            }
            
            uiOptions.ShowOptions(new List<Option>()
            {
                new Option()
                {
                    name = "Resume",
                    callback = _ =>
                    {
                        Close();
                    },
                },
                new Option()
                {
                    name = "Quest",
                    callback = _ =>
                    {
                        Close();
                    },
                },
                new Option()
                {
                    name = "Items",
                    callback = _ =>
                    {
                        Close();
                    },
                },
                new Option()
                {
                    name = "Exit",
                    callback = _ =>
                    {
                        Close();
                    },
                }
            });
        }

        private void Close()
        {
            window.Close();
            canOpenDisabledFrames = 10;
        }
    }
}