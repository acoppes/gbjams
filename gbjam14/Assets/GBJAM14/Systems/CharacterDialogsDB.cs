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
            public string characterId;
            
            public string[] requirements;
            public string[] status;
            
            public List<string> texts;
        }

        private List<DialogData> characterDialogs = new List<DialogData>();

        // private Dictionary<string, DialogData> characterDialogs = new Dictionary<string, DialogData>();

        private void Awake()
        {
            characterDialogs.Add(new DialogData()
            {
                id = "sam_dialog_start",
                characterId = "sam",
                texts = new List<string>()
                {
                    "Hey you, come here!",
                    "I have a secret to tell you!",
                    "You know.... I see dead people... And also I know where their tresures are."
                }
            });
            
            characterDialogs.Add(new DialogData()
            {
                id = "robert_dialog_start",
                characterId = "robert",
                requirements = new [] { "-the_box" },
                texts = new List<string>()
                {
                    "Oh, so you came here for treasures?",
                    "Well, I am looking for a special item.",
                    "It is really dear to me, I named it <The Box>.",
                    "If you happen to find it, I will reward you really well."
                }
            });
            
            characterDialogs.Add(new DialogData()
            {
                id = "robert_dialog_end",
                characterId = "robert",
                requirements = new [] { "+the_box" },
                status = new [] { "+the_amulet", "-the_box" },
                texts = new List<string>()
                {
                    "Oh, yes! my box",
                    "Thank you very much, here, take this amulet.",
                    "It will be handy if you happen to encounter werewolves."
                }
            });
            
            characterDialogs.Add(new DialogData()
            {
                id = "the_box_picked_robert",
                characterId = "the_box",
                requirements = new [] { "+robert_dialog_start" },
                texts = new List<string>
                {
                    "So this is <The Box> Robert was talking about",
                    "I must hurry and return it to him.",
                    "He said something about a good reward."
                }
            });
            
            characterDialogs.Add(new DialogData()
            {
                id = "the_box_picked_norobert",
                characterId = "the_box",
                requirements = new [] { "-robert_dialog_start" },
                texts = new List<string>
                {
                    "What is this thing? It looks important.",
                    "I must ask people around."
                }
            });
        }

        public DialogData GetDialog(string characterCharacterId, List<string> inventory)
        {
            foreach (var characterDialog in characterDialogs)
            {
                if (!characterDialog.characterId.Equals(characterCharacterId,
                        StringComparison.InvariantCultureIgnoreCase))
                    continue;

                // check for requirements in inventory

                var matchRequirements = true;

                if (characterDialog.requirements != null)
                {
                    foreach (var requirement in characterDialog.requirements)
                    {
                        var requirementName = requirement.Substring(1);
                        
                        if (requirement.StartsWith("-"))
                        {
                            if (inventory.Contains(requirementName))
                            {
                                matchRequirements = false;
                                break;
                            }
                        } else if (requirement.StartsWith("+"))
                        {
                            if (!inventory.Contains(requirementName))
                            {
                                matchRequirements = false;
                                break;
                            }
                        }
                    }
                }
                
                if (!matchRequirements)
                {
                    continue;
                }

                return characterDialog;
            }

            return null;
        }
    }
}