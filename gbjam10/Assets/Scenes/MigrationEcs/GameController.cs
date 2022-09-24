using System.Collections.Generic;
using GBJAM10;
using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;
using UnityEngine.Assertions;

public class GameController : ControllerBase
{
    public string followName = "Main_Camera";
    
    public Transform level;
    
    private bool initialized;

    public int startingChunks = 3;
    
    public int poolSize = 20;
    
    public LevelDataAsset levelData;

    private Transform chunksPoolParent;

    private Vector3 chunkEndPosition;

    private void GenerateNewChunk()
    {
        var childCount = chunksPoolParent.childCount;
        
        Assert.IsTrue(childCount > 0, "Cant generate chunks if empty pool");
        
        var chunkInstanceTransform =
            chunksPoolParent.GetChild(UnityEngine.Random.Range(0, childCount));

        var chunkInstance = chunkInstanceTransform.gameObject;
        chunkInstance.transform.position = chunkEndPosition;
        
        var chunkEnd = chunkInstance.transform.Find("Chunk_End");
        chunkEndPosition += new Vector3(chunkEnd.localPosition.x, 0, 0);
        
        chunkInstance.SetActive(true);
    }

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

            var chunksPoolInstance = new GameObject("~Pool_Chunks");
            chunksPoolParent = chunksPoolInstance.transform;
            
            for (int i = 0; i < poolSize; i++)
            {
                var chunkPrefab = levelData.chunksList[UnityEngine.Random.Range(0, levelData.chunksList.Count)];
                var chunkInstance = Instantiate(chunkPrefab, chunksPoolParent);
                chunkInstance.SetActive(false);
            }

            chunkEndPosition = new Vector3(-3, 0, 0);
            
            for (int i = 0; i < startingChunks; i++)
            {
                GenerateNewChunk();
                
                // var chunkPrefab = levelData.chunksList[UnityEngine.Random.Range(0, levelData.chunksList.Count)];
                // var chunkInstance = Instantiate(chunkPrefab, level);
                // chunkInstance.transform.position = chunkEndPosition;
                //
                // var chunkEnd = chunkInstance.transform.Find("Chunk_End");
                //
                // chunkEndPosition += new Vector3(chunkEnd.localPosition.x, 0, 0);
            }

            initialized = true;
        }

        for (int i = 0; i < level.childCount; i++)
        {
            var chunkTransform = level.GetChild(i);
            
            var chunkEnd = chunkTransform.Find("Chunk_End");
            if (chunkEnd.position.x < Camera.main.transform.position.x - 3)
            {
                chunkTransform.gameObject.SetActive(false);
                chunkTransform.parent = chunksPoolParent;
            }
        }

        if (Camera.main.transform.position.x + 5 > chunkEndPosition.x)
        {
            GenerateNewChunk();
        }
        
    }
}
