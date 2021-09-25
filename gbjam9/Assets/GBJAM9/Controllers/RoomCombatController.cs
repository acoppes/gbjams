using System.Linq;
using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9.Controllers
{
    public class RoomCombatController : EntityController
    {
        public GameObject[] enemyPrefabs;

        public int enemyPlayer;

        public RoomDataAsset roomDataAsset;

        private Entity rewardEntity;

        public override void OnInit(World world)
        {
            // get main player reference for later use

            var mainUnit = world.entities.FirstOrDefault(e => e.mainUnit != null);
            
            // get all exits 
            
            // get all enemy spawners
            
            // on all enemies destroyed, spawn reward near last enemy

            entity.room.state = RoomComponent.State.Fighting;
            foreach (var roomRoomSpawner in entity.room.roomSpawners)
            {
                var enemyPrefab = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)];
                if (enemyPrefab != null)
                {
                    var enemyObject = 
                        GameObject.Instantiate(enemyPrefab, roomRoomSpawner.transform.position, Quaternion.identity);
                    var enemyEntity = enemyObject.GetComponent<Entity>();
                    enemyEntity.player.player = enemyPlayer;
                }
            }
        }

        private void OnEnemiesDefeated()
        {
            entity.room.state = RoomComponent.State.WaitingReward;
            
            var rewardType = entity.room.rewardType;
            var rewardData = roomDataAsset.rewardTypes.FirstOrDefault(r => r.name.Equals(rewardType));

            if (rewardType.Equals("unknown"))
            {
                // pick random reward if reward is unkown
                var notNullRewards = roomDataAsset.rewardTypes.Where(r => r.prefab != null).ToList();
                rewardData = notNullRewards[UnityEngine.Random.Range(0, notNullRewards.Count)];
            }
            
            var rewardSpawn = entity.room.roomRewardSpawn;

            if (rewardData != null && rewardSpawn != null)
            {
                var rewardObject = GameObject.Instantiate(rewardData.prefab, rewardSpawn.transform.position, 
                    Quaternion.identity);
                rewardEntity = rewardObject.GetComponent<Entity>();
            }
        }
        
        public override void OnWorldUpdate(World world)
        {
            if (entity.room.state == RoomComponent.State.WaitingReward)
            {
                if (rewardEntity == null || rewardEntity.pickup.picked)
                {
                    entity.room.state = RoomComponent.State.Completed;
                    OpenAllExits();
                }
            } else if (entity.room.state == RoomComponent.State.Fighting)
            {
                var enemyEntities = world.entities.Where(e => e.player != null && e.player.player == enemyPlayer).ToList();
                if (enemyEntities.Count == 0)
                {
                    OnEnemiesDefeated();
                }
            }
        }

        private void OpenAllExits()
        {
            var roomList = entity.world.entities.Where(e => e.roomExit != null).ToList();
            foreach (var e in roomList)
            {
                e.roomExit.open = true;
            }
        }
    }
}