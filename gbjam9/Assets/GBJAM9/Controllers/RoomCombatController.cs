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

            entity.room.state = RoomComponent.State.WaitingReward;
            
            var rewardType = entity.room.rewardType;
            var rewardData = roomDataAsset.rewardTypes.FirstOrDefault(r => r.name.Equals(rewardType));

            if (rewardType.Equals("unknown"))
            {
                // pick random reward if reward is unkown
                var notNullRewards = roomDataAsset.rewardTypes.Where(r => r.prefab != null).ToList();
                rewardData = notNullRewards[UnityEngine.Random.Range(0, notNullRewards.Count)];
            }
            
            if (rewardData != null)
            {
                var rewardObject = GameObject.Instantiate(rewardData.prefab, mainUnit.transform.position + new Vector3(2, 0, 0), 
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