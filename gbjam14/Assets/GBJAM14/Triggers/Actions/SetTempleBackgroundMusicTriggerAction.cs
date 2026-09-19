using GBJAM14.Services;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Actions
{
    public class SetTempleBackgroundMusicTriggerAction : TriggerAction
    {
        public override string GetObjectName()
        {
            return "SetTempleMusic()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            FindFirstObjectByType<BackgroundMusicManager>().PlayTemple();
            return ITrigger.ExecutionResult.Completed;
        }
    }
}