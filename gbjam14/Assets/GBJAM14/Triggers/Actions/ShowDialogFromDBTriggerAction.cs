using System.Collections.Generic;
using GBJAM14.Systems;
using GBJAM14.UI;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Actions
{
    public class ShowDialogFromDBTriggerAction : TriggerAction
    {
        public string dialogId;
        public List<string> characters = new List<string>();

        public override string GetObjectName()
        {
            return $"ShowDialogFromDB({dialogId}, {string.Join(';', characters)})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var characterDialogsDB= FindFirstObjectByType<CharacterDialogsDB>();
            var dialogData = characterDialogsDB.GetDialog(dialogId);
            var dialog = FindFirstObjectByType<GameUIManager>().uiDialog;
            dialog.ShowDialog(dialogData, characters);
            return ITrigger.ExecutionResult.Completed;
        }
    }
}