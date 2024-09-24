using System;
using Gemserk.Utilities;
using UnityEngine;

namespace GBJAM12
{
    public class PlayerStatus : MonoBehaviour
    {
        public Cooldown failCooldown;
        public Cooldown regenerateCooldown;
        
        public int totalFailedNotes;
        public int failedNotes;
        
        public int totalCompletedNotes;

        public int notesToCompleteCombo = 5;
        public int comboCompletedNotes;

        public void OnCompletedNote()
        {
            totalCompletedNotes++;
            comboCompletedNotes++;
            
            // if (comboCompletedNotes >= notesToCompleteCombo)
            // {
            //     comboCompletedNotes = 0;
            //     failedNotes--;
            //     if (failedNotes < 0)
            //     {
            //         failedNotes = 0;
            //     }
            // }
        }

        public void OnFailedNote()
        {
            totalFailedNotes++;
            
            if (failCooldown.IsReady)
            {
                failedNotes++;
                failCooldown.Reset();

                comboCompletedNotes = 0;
                
                regenerateCooldown.Reset();
            }
        }

        private void FixedUpdate()
        {
            failCooldown.Increase(Time.deltaTime);
            regenerateCooldown.Increase(Time.deltaTime);
            
            if (regenerateCooldown.IsReady)
            {
                regenerateCooldown.Reset();
                
                failedNotes--;
                if (failedNotes < 0)
                {
                    failedNotes = 0;
                }
            }
        }
    }
}