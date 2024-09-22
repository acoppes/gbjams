using GBJAM12.Scenes;
using Gemserk.Triggers;
using MyBox;

namespace GBJAM12.Triggers
{
    public class SetCurrentLevelTriggerAction : TriggerAction
    {
        public enum ActionType
        {
            Set = 0,
            Next = 1
        }

        public ActionType actionType = ActionType.Set;
        
        [ConditionalField(nameof(actionType), false, ActionType.Set)]
        public int currentLevel;
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            if (actionType == ActionType.Set)
            {
                GameController.currentLevel = currentLevel;
            }
            else
            {
                GameController.currentLevel += 1;
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}