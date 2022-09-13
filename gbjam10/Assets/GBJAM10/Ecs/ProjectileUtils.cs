using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace GBJAM10.Ecs
{
    public struct ProjectileParameters
    {
        public IEntityDefinition definition;
        
        public Vector2 position;
        public Vector2 direction;
        public int player;
        
        // more optional stuff? maybe with a callback?
        
        // public Action<World, Entity> extraStuff
    }
    
    public static class ProjectileUtils
    {
        public static Entity Fire(Gemserk.Leopotam.Ecs.World world, ProjectileParameters projectileParameters)
        {
            var projectileEntity = world.CreateEntity(projectileParameters.definition);
            
            ref var projectileComponent = ref world.GetComponent<ProjectileComponent>(projectileEntity);

            projectileComponent.startPosition = projectileParameters.position;
            projectileComponent.startDirection = projectileParameters.direction;

            if (world.HasComponent<PlayerComponent>(projectileEntity))
            {
                var player = world.GetComponent<PlayerComponent>(projectileEntity);
                player.player = projectileParameters.player;
            }
            
            // Override target effects targetings, etc.

            return projectileEntity;
        }
    }
}