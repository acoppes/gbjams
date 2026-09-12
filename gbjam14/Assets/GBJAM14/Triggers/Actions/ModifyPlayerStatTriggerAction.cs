using Gemserk.Triggers;

namespace GBJAM14.Triggers.Actions
{
    public class ModifyPlayerStatTriggerAction : TriggerAction
    {
        public string stat;
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            GameParameters.saveGame.ModifyStat(stat);
            return ITrigger.ExecutionResult.Completed;
        }
    }
}