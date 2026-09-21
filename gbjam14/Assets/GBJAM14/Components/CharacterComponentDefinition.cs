using GBJAM14.Systems;
using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct DialogComponent : IEntityComponent
    {
        public Entity source;
        public Entity target;
        
        public string characterId;
        public string dialogId;
        
        public CharacterDialogsDB.DialogData dialogData;
        
        public bool completed;
        public bool showOptionsOnComplete;
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
            entity.AddOrSet(new CharacterComponent()
            {
                characterId = characterId.Trim()
            });
        }
    }
}