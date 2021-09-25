using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9.Controllers
{
    public class RoomNekosamaController : EntityController
    {
        public Entity nekosama;

        public override void OnInit(World world)
        {
            
        }
        
        public override void OnWorldUpdate(World world)
        {
            var nekonin = world.GetSingleton("Nekonin");

            if (nekonin == null)
            {
                return;
            }
            
            var game = world.GetSingleton("Game");

            if (Vector2.Distance(nekonin.transform.position, nekosama.transform.position) < 2)
            {
                game.game.state = GameComponent.State.Victory;
                nekonin.input.enabled = false;
            }
        }
        
    }
}