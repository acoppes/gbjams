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
        readonly EcsFilterInject<Inc<WeaponComponent, HasLookingDirectionIndicatorComponent>, Exc<DisabledComponent>> 
            weaponIndicatorFilter = default;
        
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

                var attachPoint = attachPoints.Get("weapon");
                ref var weaponPosition = ref weapons.weaponEntity.Get<PositionComponent>();

                if (weapons.inverted)
                {
                    weaponPosition.value = attachPoint.entityPosition + attachPoint.localPosition * -1f;
                }
                else
                {
                    weaponPosition.value = attachPoint.position;
                }
            }
            
            foreach (var entity in weaponIndicatorFilter.Value)
            {
                var weapon = weaponIndicatorFilter.Pools.Inc1.Get(entity);
                ref var lookingIndicator = ref weaponIndicatorFilter.Pools.Inc2.Get(entity);

                lookingIndicator.visiblity =
                    weapon.charging ? ModelComponent.Visiblity.Visible : ModelComponent.Visiblity.Hidden; 
            }
        }
    }
}