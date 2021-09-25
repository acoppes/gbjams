namespace GBJAM9.Controllers
{
    public class HudController : EntityController
    {
        // health 
        
        public override void OnWorldUpdate(World world)
        {
            var nekonin = world.GetSingleton("Nekonin");

            // health.update(nekonin.health);
            
            

        }
    }
}