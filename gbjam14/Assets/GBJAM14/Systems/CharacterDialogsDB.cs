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
            public string id;
            public List<string> texts;
        }

        private Dictionary<string, DialogData> characterDialogs = new Dictionary<string, DialogData>();

        private void Awake()
        {
            characterDialogs["sam"] = new DialogData()
            {
                id = "sam_dialog_start",
                texts = new List<string>()
                {
                    "Hey you, come here!",
                    "I have a secret to tell you!",
                    "You know.... I see dead people... And also I know where their tresures are."
                }
            };
            
            characterDialogs["robert"] = new DialogData()
            {
                id = "robert_dialog_start",
                texts = new List<string>()
                {
                    "Oh, so you came here for <THE TREASURE>",
                    "Well, you might encounter some... obstacles.",
                    "Good luck with that."
                }
            };
        }

        public DialogData GetDialog(string characterCharacterId)
        {
            return characterDialogs.GetValueOrDefault(characterCharacterId);
        }
    }
}