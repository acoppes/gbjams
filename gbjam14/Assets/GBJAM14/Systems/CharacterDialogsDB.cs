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
            public string[] output;
            
            // public string[] characters;
            
            public List<string> texts;
        }

        private List<DialogData> dialogs = new List<DialogData>();
        
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

        public void AddDialogData(DialogData dialogData)
        {
            dialogs.Add(dialogData);
        }
    }
}