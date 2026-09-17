using Game.Components;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;

namespace GBJAM14.Controllers
{
    public class ArrowController : ControllerBase, IInit
    {
        public void OnInit(World world, Entity entity)
        {
            var physics2DComponent = entity.Get<Physics2dComponent>();
            physics2DComponent.collisionsEventsDelegate.onCollisionEnter += OnEntityCollision;
        }

        private void OnEntityCollision(World world, Entity entity, IEntityCollisionDelegate.EntityCollision entityCollision)
        {
            entity.Get<DestroyableComponent>().destroy = true;
        }
    }
}