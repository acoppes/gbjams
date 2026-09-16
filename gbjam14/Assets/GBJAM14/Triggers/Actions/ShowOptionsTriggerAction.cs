using System;
using System.Collections.Generic;
using System.Linq;
using GBJAM14.UI;
using Gemserk.Triggers;

namespace GBJAM14.Triggers.Actions
{
    public class ShowOptionsTriggerAction : TriggerAction
    {
        [Serializable]
        public struct OptionData
        {
            public string name;
            public bool disabled;
        }

        public List<OptionData> options = new List<OptionData>();
        
        public override string GetObjectName()
        {
            return $"ShowOptions({string.Join(';', options.Select(o => o.name))})";
        }
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var optionsUI = FindFirstObjectByType<GameUIManager>().genericOptions;
            optionsUI.ShowOptions(options.Select(o => new Option()
            {
                name = o.name,
                disabled = o.disabled
            }).ToList());
            return ITrigger.ExecutionResult.Completed;
        }
    }
}