using GBJAM14.Components;
using GBJAM14.UI;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace GBJAM14.Systems
{
    public class CharacterInteractionSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<CharacterIdComponent, CanBeInteractedComponent>, Exc<DisabledComponent>> 
            interactableCharacters = default;

        private CharacterDialogsDB characterDialogsDB;
        
        public void Init(EcsSystems systems)
        {
            characterDialogsDB = FindFirstObjectByType<CharacterDialogsDB>();
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in interactableCharacters.Value)
            {
                var character = interactableCharacters.Pools.Inc1.Get(e);
                ref var interacted = ref interactableCharacters.Pools.Inc2.Get(e);

                if (interacted.interactPending)
                {
                    interacted.interactPending = false;
                    
                    var dialog = FindFirstObjectByType<UIDialog>();
                    if (dialog.window.IsClosed())
                    {
                        var text = characterDialogsDB.GetDialog(character.characterId);
                        dialog.ShowDialog(text);
                    }
                    
                    return;
                }
            }
        }


    }
}