using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class UnitInstanceParameter : MonoBehaviour, IEntityInstanceParameter
{
    public bool controllable = false;
    
    public void Apply(World world, int entity)
    {
        ref var position = ref world.GetComponent<PositionComponent>(entity);
        position.value = transform.position;

        if (!controllable)
        {
            ref var playerInput = ref world.GetComponent<PlayerInputComponent>(entity);
            playerInput.disabled = true;
        }
    }
}