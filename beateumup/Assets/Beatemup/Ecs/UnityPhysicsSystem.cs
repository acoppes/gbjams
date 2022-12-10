using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class UnityPhysicsSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        public void Init(EcsSystems systems)
        {
            Physics.autoSimulation = false;
        }
        
        public void Run(EcsSystems systems)
        {
            Physics.Simulate(Time.deltaTime);
        }
    }
}