using GBJAM10.Ecs;
using GBJAM10.UI;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

public class GameHudController : ControllerBase
{
    private static readonly int visibleHash = Animator.StringToHash("visible");
    
    public override void OnUpdate(float dt)
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
        var abilitiesComponent = world.GetComponent<AbilitiesComponent>(mainCharacterEntity);

        var healthUI = instance.GetComponentInChildren<HealthUI>();
        
        if (healthUI != null)
        {
            healthUI.SetHealth(healthComponent.current, healthComponent.total);
        }

        var skillsUI = instance.GetComponentInChildren<SkillsUI>();

        if (skillsUI != null)
        {
            skillsUI.SetAbilities(abilitiesComponent.GetAbility("MainAbility").CooldownFactor, 
                abilitiesComponent.GetAbility("SecondaryAbility").CooldownFactor);
        }
        
        animator.SetBool(visibleHash, modelComponent.IsVisible);
    }
}