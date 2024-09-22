using GBJAM12.Scenes;
using Gemserk.Triggers;

namespace GBJAM12.Triggers
{
    public class SetCurrentLevelTriggerAction : TriggerAction
    {
        public int currentLevel;
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            GameController.currentLevel = currentLevel;
            return ITrigger.ExecutionResult.Completed;
        }
    }
}