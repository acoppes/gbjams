using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;

namespace GBJAM14.Components
{
    public class ArrowTrapInstanceParameter : EntityInstanceParameterBase
    {
        public float fireCooldownStart;
        public float fireCooldown;
        public float reloadCooldown;
        
        public override void Apply(World world, Entity entity)
        {
            ref var arrowTrap = ref entity.Get<ArrowTrapComponent>();
            arrowTrap.fireCooldown = new Cooldown(fireCooldown)
            {
                current = fireCooldownStart
            };
            arrowTrap.reloadCooldown = new Cooldown(reloadCooldown)
            {
                current = reloadCooldown
            };
        }
    }
}