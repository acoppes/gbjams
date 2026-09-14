using Gemserk.Triggers;
using UnityEngine;

namespace GBJAM14.Triggers.Actions
{
    public class AnimatorTriggerAction : TriggerAction
    {
        public Animator targetAnimator;
        public string triggerName;
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            targetAnimator.SetTrigger(triggerName);
            return ITrigger.ExecutionResult.Completed;
        }
    }
}