using Game.Utilities;
using Gemserk.Leopotam.Ecs;

namespace GBJAM11.Systems
{
    public interface IEntityCollisionEvent
    {
        void OnEntityCollision(World world, Entity entity, IEntityCollisionDelegate.EntityCollision entityCollision);
    }
}