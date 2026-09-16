using GBJAM14.GamePlay;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Actions
{
    public class SaveSavegameTriggerAction : WorldTriggerAction
    {
        public override string GetObjectName()
        {
            return "SaveGame.Save()";
        }
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            ActionUtils.SaveGameSave(world);
            return ITrigger.ExecutionResult.Completed;
        }
    }
}