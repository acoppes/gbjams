using UnityEngine;

namespace GBJAM9.Controllers
{
    public class RoomCombatController : EntityController
    {
        public GameObject[] enemyPrefabs;

        public int enemyPlayer;

        public override void OnInit(World world)
        {
            // get main player reference for later use
            
            // get all exits 
            
            // get all enemy spawners
            
            // on all enemies destroyed, spawn reward near last enemy
            
            // on pick reward, open doors

            entity.room.state = RoomComponent.State.Fighting;
        }

        public override void OnWorldUpdate(World world)
        {
            
        }
    }
}