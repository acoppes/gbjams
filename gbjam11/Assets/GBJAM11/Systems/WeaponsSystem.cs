using Game.Components;
using GBJAM11.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace GBJAM11.Systems
{
    public class WeaponsSystem : BaseSystem, IEntityCreatedHandler, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<AttachPointsComponent, WeaponsComponent>, Exc<DisabledComponent>> filter = default;
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (entity.Has<WeaponsComponent>())
            {
                ref var weapons = ref entity.Get<WeaponsComponent>();
                weapons.weaponEntity = world.CreateEntity(weapons.defaultWeaponDefinition);
                weapons.weaponEntity.Get<WeaponComponent>().holder = entity;
            }
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var attachPoints = ref filter.Pools.Inc1.Get(entity);
                ref var weapons = ref filter.Pools.Inc2.Get(entity);
                weapons.weaponEntity.Get<PositionComponent>().value = attachPoints.Get("weapon").position; 
            }
        }

    }
}