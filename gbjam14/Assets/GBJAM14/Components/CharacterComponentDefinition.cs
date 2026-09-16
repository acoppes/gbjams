using GBJAM14.Systems;
using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct DialogComponent : IEntityComponent
    {
        public Entity sourceEntity;
        
        public string characterId;
        public string dialogId;
        
        public CharacterDialogsDB.DialogData dialogData;
        
        public bool completed;
    }
    
    public struct CharacterComponent : IEntityComponent
    {
        public string characterId;
        // public string name;
    }
    
    public class CharacterComponentDefinition : ComponentDefinitionBase, IEntityInstanceParameter
    {
        public string characterId;
        // public string characterName;
        
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new CharacterComponent()
            {
                characterId = characterId.Trim(),
                // name = characterName
            });
        }
    }
}