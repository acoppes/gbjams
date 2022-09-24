using GBJAM10;
using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public class GameController : ControllerBase
{
    public string followName = "Main_Camera";
    
    public Transform level;
    
    private bool initialized;

    public int maxChunks = 500;
    public LevelDataAsset levelData;

    public override void OnUpdate(float dt)
    {
        if (!initialized)
        {
            world.sharedData.sharedData = new SharedGameData
            {
                activePlayer = 0
            };
            
            var followEntity = world.GetEntityByName(followName);
            if (followEntity != Entity.NullEntity)
            {
                var cameraFollow = FindObjectOfType<CameraFollow>();
                var model = world.GetComponent<UnitModelComponent>(followEntity);
                cameraFollow.followTransform = model.instance.transform;
            }

            var startPosition = new Vector3(0, 0, 0);
            for (int i = 0; i < maxChunks; i++)
            {
                var chunkPrefab = levelData.chunksList[UnityEngine.Random.Range(0, levelData.chunksList.Count)];
                var chunkInstance = Instantiate(chunkPrefab, level);
                chunkInstance.transform.position = startPosition;
                
                startPosition += new Vector3(50, 0, 0);
            }

            initialized = true;
        }
        
    }
}
