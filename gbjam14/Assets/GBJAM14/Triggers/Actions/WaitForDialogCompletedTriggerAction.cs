using GBJAM14.UI;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Actions
{
    public class WaitForDialogCompletedTriggerAction : TriggerAction
    {
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var uiDialog = FindFirstObjectByType<GameUIManager>().uiDialog;
            
            if (uiDialog && uiDialog.completed && !uiDialog.waiting)
            {
                return ITrigger.ExecutionResult.Completed;
            }

            return ITrigger.ExecutionResult.Running;
        }
    }
}