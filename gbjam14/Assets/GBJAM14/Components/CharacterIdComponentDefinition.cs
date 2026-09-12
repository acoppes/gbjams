using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct CharacterIdComponent : IEntityComponent
    {
        public string characterId;
    }
    
    public class CharacterIdComponentDefinition : ComponentDefinitionBase
    {
        public string characterId;
        
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new CharacterIdComponent()
            {
                characterId = characterId
            });
        }
    }
}