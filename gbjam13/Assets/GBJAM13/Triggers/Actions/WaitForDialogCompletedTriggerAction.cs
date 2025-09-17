using GBJAM13.UI;
using Gemserk.Triggers;

namespace GBJAM13.Triggers.Actions
{
    public class WaitForDialogCompletedTriggerAction : TriggerAction
    {
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var dialog = FindFirstObjectByType<UIDialog>();
            
            if (dialog && dialog.completed)
            {
                return ITrigger.ExecutionResult.Completed;
            }

            return ITrigger.ExecutionResult.Running;
        }
    }
}