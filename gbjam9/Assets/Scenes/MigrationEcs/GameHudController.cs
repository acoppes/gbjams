using GBJAM9.Ecs;
using GBJAM9.UI;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public class GameHudController : MonoBehaviour, IController
{
    private static readonly int visibleHash = Animator.StringToHash("visible");
    
    public void OnUpdate(float dt, World world, int entity)
    {
        ref var modelComponent = ref world.GetComponent<UnitModelComponent>(entity);

        var instance = modelComponent.instance;
        var animator = instance.GetComponent<Animator>();

        var mainCharacterEntity = world.GetEntityByName("Main_Character");

        if (mainCharacterEntity == Entity.NullEntity)
        {
            animator.SetBool(visibleHash, false);
            return;
        }
        
        var healthComponent = world.GetComponent<HealthComponent>(mainCharacterEntity);

        var healthUI = instance.GetComponentInChildren<HealthUI>();
        
        if (healthUI != null)
        {
            healthUI.SetHealth(healthComponent.current, healthComponent.total);
        }
        
        animator.SetBool(visibleHash, modelComponent.IsVisible);
    }
}