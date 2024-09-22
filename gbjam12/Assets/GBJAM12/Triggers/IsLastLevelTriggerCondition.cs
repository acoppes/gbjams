using GBJAM12.Scenes;
using Gemserk.Triggers;
using UnityEngine;

namespace GBJAM12.Triggers
{
    public class IsLastLevelTriggerCondition : TriggerCondition
    {
        public override bool Evaluate(object activator = null)
        {
            var current = GameController.currentLevel;
            var gameController = GameObject.FindAnyObjectByType<GameController>();

            return current >= gameController.gameConfiguration.levels.Count - 1;
            
        }
    }
}