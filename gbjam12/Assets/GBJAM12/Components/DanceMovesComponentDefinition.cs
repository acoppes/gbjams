using Gemserk.Leopotam.Ecs;

namespace GBJAM12.Components
{
    public struct IncomingNote
    {
        public bool hasIncomingNote;
        // public int durationInTicks;
    }
    
    public struct DanceMovesComponent : IEntityComponent
    {
        public IncomingNote[] incomingNotes;

        public bool n1 => incomingNotes[0].hasIncomingNote;
        public bool n2 => incomingNotes[1].hasIncomingNote;
        public bool n3 => incomingNotes[2].hasIncomingNote;
        
    }
    
    public class DanceMovesComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new DanceMovesComponent()
            {
                incomingNotes = new IncomingNote[3]
            });
        }
    }
}