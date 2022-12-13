using Beatemup.Ecs;
using Gemserk.Gameplay.Signals;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;

namespace Beatemup.Controllers
{
    public class TeamKillCountController : ControllerBase, IInit
    {
        public SignalAsset onEntityDeath;
        
        public void OnInit()
        {
            if (onEntityDeath != null)
            {
                onEntityDeath.Register(OnEntityDeath);
            }
            
            // TODO: on destroy method for controllers instead of using monobehaviour destroy?
        }
        
        private void OnDestroy()
        {
            if (onEntityDeath != null)
            {
                onEntityDeath.Unregister(OnEntityDeath);
            }
        }

        private void OnEntityDeath(object userdata)
        {
            var deathEntity = (Entity) userdata;
            var hitPoints = world.GetComponent<HitPointsComponent>(deathEntity);

            var playerComponent = world.GetComponent<PlayerComponent>(entity);
            ref var killCountComponent = ref world.GetComponent<KillCountComponent>(entity);
            
            foreach (var hit in hitPoints.hits)
            {
                if (world.Exists(hit.source) && world.HasComponent<PlayerComponent>(hit.source))
                {
                    ref var sourcePlayerComponent = ref world.GetComponent<PlayerComponent>(hit.source);
                    if (sourcePlayerComponent.player == playerComponent.player)
                    {
                        killCountComponent.count++;
                        return;
                    }
                }
            }
        }

        public override void OnUpdate(float dt)
        {
            
        }

    }
}