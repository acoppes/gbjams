using GBJAM9;
using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;
using World = Gemserk.Leopotam.Ecs.World;

public class TestEcsSceneController : MonoBehaviour, IController
{
    private bool initialized;
    
    public void OnUpdate(float dt, World world, int entity)
    {
        if (!initialized)
        {
            var mainCharacterEntity = world.GetEntityByName("Main_Character");
            if (mainCharacterEntity != Entity.NullEntity)
            {
                var cameraFollow = FindObjectOfType<CameraFollow>();
                var model = world.GetComponent<UnitModelComponent>(mainCharacterEntity);
                cameraFollow.followTransform = model.instance.transform;
            }
        }
        
    }
}
