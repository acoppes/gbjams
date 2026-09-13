using System.Collections.Generic;
using GBJAM14.Systems;
using GBJAM14.UI;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Actions
{
    public class ShowDialogTriggerAction : TriggerAction
    {
        public CharacterDialogsDB.DialogData dialogData;
        public List<string> characters = new List<string>();
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var dialog = FindFirstObjectByType<UIDialog>();
            dialog.ShowDialog(dialogData, characters);
            return ITrigger.ExecutionResult.Completed;
        }
    }
}