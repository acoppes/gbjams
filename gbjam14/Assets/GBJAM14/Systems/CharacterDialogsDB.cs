using System;
using System.Collections.Generic;
using System.Linq;
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
            
            // public string[] characters;
            
            public List<string> texts;
        }

        private List<DialogData> dialogs = new List<DialogData>();

        // private Dictionary<string, DialogData> characterDialogs = new Dictionary<string, DialogData>();

        private void Awake()
        {
            dialogs.Add(new DialogData()
            {
                id = "game_start",
                characterId = "main_character",
                texts = new List<string>()
                {
                    "[0]: Ok, I finally arrived",
                    "[0]: Grandma shared so many stories about Hollow island.",
                    "[0]: Wish she was here with me to find it...",
                    "[0]: The GOLDEN TREASURE...",
                    "[0]: To finally become the best archeologist."
                }
            });
            
            dialogs.Add(new DialogData()
            {
                id = "mrtoad_start",
                characterId = "mrtoad",
                texts = new List<string>()
                {
                    "[1]: Hey [0], come here!",
                    "[1]: I have a secret to tell you!",
                    "[0]: Ok [1], I am listening!",
                    "[1]: You know.... I see dead people...",
                    "[1]: And also I know where their treasures are."
                }
            });
            
            dialogs.Add(new DialogData()
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
            
            dialogs.Add(new DialogData()
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
            
            dialogs.Add(new DialogData()
            {
                id = "the_box_picked_robert",
                characterId = "the_box",
                requirements = new [] { "+robert_dialog_start" },
                texts = new List<string>
                {
                    "[0]: So this is <The Box> Robert was talking about",
                    "[0]: I must hurry and return it to him.",
                    "[0]: He said something about a good reward."
                }
            });
            
            dialogs.Add(new DialogData()
            {
                id = "the_box_picked_norobert",
                characterId = "the_box",
                requirements = new [] { "-robert_dialog_start" },
                texts = new List<string>
                {
                    "[0]: What is this thing? It looks important.",
                    "[0]: I must ask people around."
                }
            });
            
            dialogs.Add(new DialogData()
            {
                id = "random1",
                characterId = "lost_soul1",
                requirements = new [] { "-lost_soul1" },
                status = new [] { "+lost_soul1" },
                texts = new List<string>
                {
                    "[1]: Hey Stranger, Welcome to Hollow Island.",
                    "[1]: What's your name?",
                    "[0]: My name is [0] D. Topson, I am the best Archeologist.",
                    "[1]: Oh, wow, that's ... ehm great. See ya around.",
                }
            });
            
            dialogs.Add(new DialogData()
            {
                id = "random1",
                characterId = "lost_soul1",
                requirements = new [] { "+lost_soul1" },
                texts = new List<string>
                {
                    "[1]: You again?",
                }
            });
            
            dialogs.Add(new DialogData()
            {
                id = "random2",
                characterId = "lost_soul2",
                texts = new List<string>
                {
                    "[1]: Hey, are you new in Hollow Island?",
                    "[1]: We have the best ghost steaks!",
                }
            });
        }
        
        public List<DialogData> GetCharacterDialogs(string characterId, List<string> inventory = null)
        {
            var characterDialogs = new List<DialogData>();
            
            foreach (var characterDialog in dialogs)
            {
                if (!characterDialog.characterId.Equals(characterId,
                        StringComparison.InvariantCultureIgnoreCase))
                    continue;

                // check for requirements in inventory

                if (inventory != null)
                {
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
                }
                
                characterDialogs.Add(characterDialog);
            }

            return characterDialogs;
        }

        public DialogData GetDialog(string dialogId)
        {
            return dialogs.FirstOrDefault(d => 
                d.id.Equals(dialogId, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}