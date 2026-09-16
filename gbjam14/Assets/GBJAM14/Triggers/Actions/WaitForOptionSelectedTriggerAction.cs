using GBJAM14.UI;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Actions
{
    public class WaitForOptionSelectedTriggerAction : TriggerAction
    {
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var uiEventOptions = FindFirstObjectByType<GameUIManager>().genericOptions;
            
            if (uiEventOptions && uiEventOptions.optionSelected)
            {
                return ITrigger.ExecutionResult.Completed;
            }

            return ITrigger.ExecutionResult.Running;
        }
    }
}