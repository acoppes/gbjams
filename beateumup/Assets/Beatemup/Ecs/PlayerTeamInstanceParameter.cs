using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;
using UnityEngine.Serialization;

namespace Beatemup.Ecs
{
    public class PlayerTeamInstanceParameter : MonoBehaviour, IEntityInstanceParameter
    {
        [FormerlySerializedAs("player")] 
        public int team;
        
        public void Apply(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            ref var playerComponent = ref world.GetComponent<PlayerComponent>(entity);
            playerComponent.player = team;
        }
    }
}