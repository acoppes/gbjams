using GBJAM13.UI;
using Gemserk.Triggers;

namespace GBJAM13.Triggers.Actions
{
    public class WaitForOptionSelectedTriggerAction : TriggerAction
    {
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var uiEventOptions = FindFirstObjectByType<UIOptions>();
            
            if (uiEventOptions && uiEventOptions.optionSelected)
            {
                return ITrigger.ExecutionResult.Completed;
            }

            return ITrigger.ExecutionResult.Running;
        }
    }
}