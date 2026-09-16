using GBJAM14.GamePlay;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Actions
{
    public class LoadSavegameTriggerAction : WorldTriggerAction
    {
        public override string GetObjectName()
        {
            return "SaveGame.Load()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            ActionUtils.SaveGameLoad(world);
            return ITrigger.ExecutionResult.Completed;
        }
    }
}