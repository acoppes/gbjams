using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct DialogComponent : IEntityComponent
    {
        public string characterId;
        public string dialogId;
        public bool completed;
    }
    
    public struct CharacterComponent : IEntityComponent
    {
        public string characterId;
    }
    
    public class CharacterComponentDefinition : ComponentDefinitionBase, IEntityInstanceParameter
    {
        public string characterId;
        
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new CharacterComponent()
            {
                characterId = characterId
            });
        }
    }
}