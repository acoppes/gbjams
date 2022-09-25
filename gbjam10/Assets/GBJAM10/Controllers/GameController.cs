using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;
using UnityEngine.Assertions;

namespace GBJAM10.Controllers
{
    public class GameController : ControllerBase
    {
        public Transform level;
    
        private bool initialized;

        public int startingChunks = 3;

        public float initialGameSpeed = 5;
    
        public int poolSize = 20;
    
        public LevelDataAsset levelData;

        private Transform chunksPoolParent;

        private Vector3 chunkEndPosition;

        private Entity mainCharacter;
        private Entity mainCamera;
        private Entity mainEnemy;

        private void GenerateNewChunk()
        {
            var childCount = chunksPoolParent.childCount;
        
            Assert.IsTrue(childCount > 0, "Cant generate chunks if empty pool");
        
            var chunkInstanceTransform =
                chunksPoolParent.GetChild(UnityEngine.Random.Range(0, childCount));
            chunkInstanceTransform.SetParent(level);
        
            var chunkInstance = chunkInstanceTransform.gameObject;
            chunkInstance.transform.position = chunkEndPosition;
        
            var chunkEnd = chunkInstance.transform.Find("Chunk_End");
            chunkEndPosition += new Vector3(chunkEnd.localPosition.x, 0, 0);
        
            chunkInstance.SetActive(true);
        }

        private void UpdateEntitySpeed(Entity entity, float speed)
        {
            if (!world.HasComponent<UnitMovementComponent>(entity))
                return;
        
            ref var movementComponent = ref world.GetComponent<UnitMovementComponent>(entity);
            movementComponent.speed = speed;
        }

        public override void OnUpdate(float dt)
        {
            if (!initialized)
            {
                world.sharedData.sharedData = new SharedGameData
                {
                    activePlayer = 0
                };
            
                mainCamera = world.GetEntityByName("Main_Camera");
                mainCharacter = world.GetEntityByName("Main_Character");
                mainEnemy = world.GetEntityByName("Main_Enemy");
            
                if (mainCamera != Entity.NullEntity)
                {
                    var cameraFollow = FindObjectOfType<CameraFollow>();
                    var model = world.GetComponent<UnitModelComponent>(mainCamera);
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
            
                UpdateEntitySpeed(mainCamera, initialGameSpeed);
                UpdateEntitySpeed(mainCharacter, initialGameSpeed);
                UpdateEntitySpeed(mainEnemy, initialGameSpeed);
            }

            var mainCameraPosition = world.GetComponent<PositionComponent>(mainCamera);

            for (int i = 0; i < level.childCount; i++)
            {
                var chunkTransform = level.GetChild(i);
            
                var chunkEnd = chunkTransform.Find("Chunk_End");
                if (chunkEnd.position.x < mainCameraPosition.value.x - 5)
                {
                    chunkTransform.gameObject.SetActive(false);
                    chunkTransform.parent = chunksPoolParent;
                }
            }

            if (mainCameraPosition.value.x + 5 > chunkEndPosition.x)
            {
                GenerateNewChunk();
            }
        
        }
    }
}
