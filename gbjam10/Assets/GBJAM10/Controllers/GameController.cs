using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace GBJAM10.Controllers
{
    public class GameController : ControllerBase, IEntityDestroyed, IInit
    {
        public Transform level;
    
        public int startingChunks = 3;

        public float initialGameSpeed = 5;
    
        public int poolSize = 20;
    
        public LevelDataAsset levelData;

        private Transform chunksPoolParent;

        private Vector3 chunkEndPosition;

        private Entity mainCharacter;
        private Entity mainCamera;
        private Entity mainEnemy;

        public float gameOverDuration = 3.0f;
        public GameObject defeatDefinition;

        public float victoryDuration = 3.0f;

        public string restartSceneName = "Game";
        public string endingSceneName = "EndingScene";

        public GameObject bossDeathDefinition;
        
        public void OnEntityDestroyed(Entity e)
        {
            if (e == mainCharacter)
            {
                mainCharacter = Entity.NullEntity;

                var states = world.GetComponent<StatesComponent>(entity);
                states.EnterState("GameOver");
                
                // show gameover object
                var gameHudEntity = world.GetEntityByName("Game_Hud");
                if (gameHudEntity != Entity.NullEntity)
                {
                    ref var model = ref world.GetComponent<UnitModelComponent>(gameHudEntity);
                    model.visiblity = UnitModelComponent.Visiblity.Hidden;
                }

                ref var bossHealth = ref world.GetComponent<HealthComponent>(mainEnemy);
                bossHealth.invulnerableTime = 10;
                bossHealth.invulnerableCurrent = 10;

                world.CreateEntity(defeatDefinition.GetInterface<IEntityDefinition>(), null);
            }
            
            if (e == mainEnemy)
            {
                var states = world.GetComponent<StatesComponent>(entity);
                states.EnterState("Victory");
                
                // show gameover object
                var gameHudEntity = world.GetEntityByName("Game_Hud");
                if (gameHudEntity != Entity.NullEntity)
                {
                    ref var model = ref world.GetComponent<UnitModelComponent>(gameHudEntity);
                    model.visiblity = UnitModelComponent.Visiblity.Hidden;
                }
                
                ref var heroHealth = ref world.GetComponent<HealthComponent>(mainCharacter);
                heroHealth.invulnerableTime = 10;
                heroHealth.invulnerableCurrent = 10;
                
                var gameModel = world.GetComponent<UnitModelComponent>(entity);
                var sfxBossDeath = gameModel.instance.transform.FindInHierarchy("Sfx_Boss_Death");
                if (sfxBossDeath != null)
                {
                    sfxBossDeath.GetComponent<AudioSource>().Play();
                }

                var bossPosition = world.GetComponent<PositionComponent>(mainEnemy);
                var bossMovement = world.GetComponent<UnitMovementComponent>(mainEnemy);
                
                var bossDeathEntity = world.CreateEntity(bossDeathDefinition.GetInterface<IEntityDefinition>(), null);
                ref var bossDeathPosition = ref world.GetComponent<PositionComponent>(bossDeathEntity);
                ref var bossDeathMovement = ref world.GetComponent<UnitMovementComponent>(bossDeathEntity);

                bossDeathPosition.value = bossPosition.value;
                bossDeathMovement.movingDirection = bossMovement.movingDirection;
                bossDeathMovement.speed = bossMovement.speed;
                
                mainEnemy = Entity.NullEntity;
            }
        }
        
        public void OnInit()
        {
            world.sharedData.sharedData = new SharedGameData
            {
                activePlayer = 0
            };

            // gameHud = world.GetEntityByName("Game_Hud");
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
        
            for (var i = 0; i < startingChunks; i++)
            {
                GenerateNewChunk();
            }
        
            UpdateEntitySpeed(mainCamera, initialGameSpeed);
            UpdateEntitySpeed(mainCharacter, initialGameSpeed);
            UpdateEntitySpeed(mainEnemy, initialGameSpeed);
        }

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
            var states = world.GetComponent<StatesComponent>(entity);

            if (states.HasState("GameOver"))
            {
                var state = states.GetState("GameOver");
                if (state.time > gameOverDuration)
                {
                    SceneManager.LoadScene(restartSceneName);
                }
            }
            
            if (states.HasState("Victory"))
            {
                var state = states.GetState("Victory");
                if (state.time > victoryDuration)
                {
                    SceneManager.LoadScene(endingSceneName);
                }
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
