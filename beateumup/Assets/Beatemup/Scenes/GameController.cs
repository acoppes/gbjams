using Beatemup.Ecs;
using Beatemup.MainMenu;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;
using UnityEngine.SceneManagement;

namespace Beatemup.Scenes
{
    public class GameController : ControllerBase, IInit, IUpdate
    {
        public float startingTime;
        public float gameOverTime;
        
        public void OnInit()
        {
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            states.EnterState("Starting");
        }
        
        public void OnUpdate(float dt)
        {
            ref var states = ref world.GetComponent<StatesComponent>(entity);

            State state;
            
            if (states.TryGetState("Starting", out state))
            {
                if (state.time > startingTime)
                {
                    states.ExitState("Starting");
                    states.EnterState("Running");
                }
                return;
            }

            if (states.TryGetState("Running", out state))
            {
                // check if players still alive...
                
                var targets = TargetingUtils.GetTargets(world, new TargetingUtils.RuntimeTargetingParameters
                {
                    player = 0,
                    checkAreaType = TargetingUtils.RuntimeTargetingParameters.CheckAreaType.Nothing,
                    playerAllianceType = TargetingUtils.PlayerAllianceType.Allies,
                    aliveType = HitPointsComponent.AliveType.Alive
                });

                if (targets.Count == 0)
                {
                    states.ExitState("Running");
                    states.EnterState("GameOver");
                }

                return;
            }
            
            if (states.TryGetState("GameOver", out state))
            {
                if (state.time > gameOverTime)
                {
                    SceneManager.LoadScene("MainMenu");
                }
                return;
            }
        }

    }
}