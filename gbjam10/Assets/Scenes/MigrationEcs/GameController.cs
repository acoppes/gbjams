using GBJAM10;
using GBJAM10.Ecs;
using GBJAM9;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;

public class GameController : ControllerBase
{
    private bool initialized;
    
    public override void OnUpdate(float dt)
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
