using System.Collections;
using System.Collections.Generic;
using Beatemup;
using Beatemup.Definitions;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class FireRandomProjectiles : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject parametersObject;

    public World world;
    
    public void Update()
    {
        var projectile = world.CreateEntity(projectilePrefab.GetInterface<IEntityDefinition>(),
            parametersObject.GetComponentsInChildren<IEntityInstanceParameter>());
        
        if (world.HasComponent<LookingDirection>(projectile))
        {
            ref var lookingDirection = ref world.GetComponent<LookingDirection>(projectile);
            lookingDirection.value = Vector2.right.Rotate(UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad);
        }
    }
}
