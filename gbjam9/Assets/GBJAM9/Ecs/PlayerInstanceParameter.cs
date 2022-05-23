using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace GBJAM9.Ecs
{
    public class PlayerInstanceParameter : MonoBehaviour, IEntityInstanceParameter
    {
        public int player;
        
        public void Apply(Gemserk.Leopotam.Ecs.World world, int entity)
        {
            world.AddComponent(entity, new PlayerComponent
            {
                player = player
            });
        }
    }
}