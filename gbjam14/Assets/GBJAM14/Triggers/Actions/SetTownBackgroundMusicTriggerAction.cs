using GBJAM14.Services;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Actions
{
    public class SetTownBackgroundMusicTriggerAction : TriggerAction
    {
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            FindFirstObjectByType<BackgroundMusicManager>().PlayTown();
            return ITrigger.ExecutionResult.Completed;
        }
    }
}