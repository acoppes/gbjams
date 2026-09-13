using GBJAM14.Systems;
using GBJAM14.UI;
using Gemserk.Triggers;
using UnityEngine;

namespace GBJAM14.Triggers.Actions
{
    public class ShowDialogTriggerAction : TriggerAction
    {
        public CharacterDialogsDB.DialogData dialogData;
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var dialog = FindFirstObjectByType<UIDialog>();
            dialog.ShowDialog(dialogData);
            return ITrigger.ExecutionResult.Completed;
        }
    }
}