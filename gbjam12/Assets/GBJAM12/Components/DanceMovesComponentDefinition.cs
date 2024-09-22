using Gemserk.Leopotam.Ecs;

namespace GBJAM12.Components
{
    public struct IncomingNote
    {
        public bool hasIncomingNote;
        public int distanceToEndInTicks;
    }
    
    public struct DanceMovesComponent : IEntityComponent
    {
        public IncomingNote[] incomingNotes;

        public int totalMistakes;
        public int currentMistakes;
        
        public bool n1 => incomingNotes[0].hasIncomingNote;
        public bool n2 => incomingNotes[1].hasIncomingNote;
        public bool n3 => incomingNotes[2].hasIncomingNote;

        public int GetLongerNote()
        {
            var longerNote = -1;
            var note = -1;
            
            for (var i = 0; i < incomingNotes.Length; i++)
            {
                if (!incomingNotes[i].hasIncomingNote)
                    continue;
                
                if (incomingNotes[i].distanceToEndInTicks > longerNote)
                {
                    longerNote = incomingNotes[i].distanceToEndInTicks;
                    note = i;
                }
            }

            return note;
        }
        
    }
    
    public class DanceMovesComponentDefinition : ComponentDefinitionBase
    {
        public int totalMistakes;
        
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new DanceMovesComponent()
            {
                incomingNotes = new IncomingNote[3],
                totalMistakes = totalMistakes,
                currentMistakes = 0
            });
        }
    }
}