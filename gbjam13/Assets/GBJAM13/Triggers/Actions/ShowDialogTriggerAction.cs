using GBJAM13.UI;
using Gemserk.Triggers;
using UnityEngine;

namespace GBJAM13.Triggers.Actions
{
    public class ShowDialogTriggerAction : TriggerAction
    {
        [TextArea(2, 5)]
        public string text;

        public bool append;
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var dialog = FindFirstObjectByType<UIDialog>();
            if (append)
            {
                dialog.AppendText(text);
            }
            else
            {
                dialog.ShowText(text);
            }
            return ITrigger.ExecutionResult.Completed;
        }
    }
}