using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

public class UnitInstanceParameter : MonoBehaviour, IEntityInstanceParameter
{
    public bool controllable = false;

    public void Apply(World world, Entity entity)
    {
        ref var position = ref world.GetComponent<PositionComponent>(entity);
        position.value = transform.position;

        if (!controllable)
        {
            if (world.HasComponent<PlayerInputComponent>(entity))
            {
                ref var playerInput = ref world.GetComponent<PlayerInputComponent>(entity);
                playerInput.disabled = true;
            }
        }
    }
}