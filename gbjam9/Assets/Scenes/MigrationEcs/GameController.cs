using GBJAM9;
using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;
using World = Gemserk.Leopotam.Ecs.World;

public class GameController : MonoBehaviour, IController
{
    private bool initialized;
    
    public void OnUpdate(float dt, World world, Entity entity)
    {
        if (!initialized)
        {
            world.sharedData.sharedData = new SharedGameData
            {
                activePlayer = 0
            };
            
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
