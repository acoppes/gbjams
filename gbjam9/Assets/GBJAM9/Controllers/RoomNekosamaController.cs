using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9.Controllers
{
    public class RoomNekosamaController : EntityController
    {
        public Entity nekosama;

        private bool completed;

        public override void OnInit(World world)
        {
            
        }
        
        public override void OnWorldUpdate(World world)
        {
            if (completed)
                return;
            
            var nekonin = world.GetSingleton("Nekonin");

            if (nekonin == null)
            {
                return;
            }
            
            var game = world.GetSingleton("Game");

            if (game == null)
                return;

            if (Vector2.Distance(nekonin.transform.position, nekosama.transform.position) < 2)
            {
                // TODO: internal state with cinematic or something, and then victory
                game.game.state = GameComponent.State.Victory;
                nekonin.input.enabled = false;
                completed = true;
            }
        }
        
    }
}