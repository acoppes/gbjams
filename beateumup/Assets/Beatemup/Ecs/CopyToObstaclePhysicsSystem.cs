﻿using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using MyBox;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class CopyToObstaclePhysicsSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var obstacleComponents = world.GetComponents<ObstacleComponent>();
            var positions = world.GetComponents<PositionComponent>();
            
            foreach (var entity in world.GetFilter<ObstacleComponent>().Inc<PositionComponent>().End())
            {
                // copy from position to body
                ref var obstacleComponent = ref obstacleComponents.Get(entity);
                var positionComponent = positions.Get(entity);

                obstacleComponent.body.position = positionComponent.value.ToVector2();
            }
        }
    }
}