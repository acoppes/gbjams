using GBJAM14.GamePlay;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Actions
{
    public class ConfigureActiveRoomCameraConfinerTriggerAction : WorldTriggerAction
    {
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            ActionUtils.ConfigureActiveRoomCameraConfiner(world);
            return ITrigger.ExecutionResult.Completed;
        }
    }
}