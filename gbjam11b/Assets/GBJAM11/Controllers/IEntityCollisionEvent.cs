using Game.Utilities;
using Gemserk.Leopotam.Ecs;

namespace GBJAM11.Controllers
{
    public interface IEntityCollisionEvent
    {
        void OnEntityCollision(World world, Entity entity, IEntityCollisionDelegate.EntityCollision entityCollision);
    }
}