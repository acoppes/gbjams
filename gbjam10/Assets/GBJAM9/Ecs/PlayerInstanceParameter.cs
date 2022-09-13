using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace GBJAM9.Ecs
{
    public class PlayerInstanceParameter : MonoBehaviour, IEntityInstanceParameter
    {
        public int player;
        
        public void Apply(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            ref var playerComponent = ref world.GetComponent<PlayerComponent>(entity);
            playerComponent.player = player;
        }
    }
}