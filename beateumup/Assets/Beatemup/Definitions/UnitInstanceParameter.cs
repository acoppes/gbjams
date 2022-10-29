using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace Beatemup.Definitions
{
    public class UnitInstanceParameter : MonoBehaviour, IEntityInstanceParameter
    {
        public int playerInput;
        
        public bool controllable = false;

        public void Apply(World world, Entity entity)
        {
            ref var position = ref world.GetComponent<PositionComponent>(entity);
            position.value = transform.position;
            
            if (controllable)
            {
                world.AddComponent(entity, new PlayerInputComponent()
                {
                    playerInput = playerInput,
                    disabled = false
                });
            }
        }
    }
}