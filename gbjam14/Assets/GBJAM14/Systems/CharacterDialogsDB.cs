using System;
using System.Collections.Generic;
using UnityEngine;

namespace GBJAM14.Systems
{
    public class CharacterDialogsDB : MonoBehaviour
    {
        [Serializable]
        public class DialogData
        {
            public List<string> texts;
        }
        
        public DialogData GetDialog(string characterCharacterId)
        {
            return new DialogData()
            {
                texts = new List<string>()
                {
                    "Hey you, come here!",
                    "I have a secret to tell you!",
                    "You know.... I see dead people... And also I know where their tresures are."
                }
            };
        }
    }
}