using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace Beatemup.Scenes
{
    public class GameSceneController : MonoBehaviour
    {
        public static int players = 1;

        public GameObject playerCharacterDefinition;
        
        // Start is called before the first frame update
        void Start()
        {
            var world = World.Instance;
            
            for (var i = 0; i < players; i++)
            {
                var playerCharacterEntity = world.CreateEntity(playerCharacterDefinition);
                
                world.AddComponent(playerCharacterEntity, new PlayerInputComponent()
                {
                    playerInput = i,
                    disabled = false
                });
                
                world.AddComponent(playerCharacterEntity, new NameComponent
                {
                    name = $"Character_Player_{i}",
                    singleton = true
                });

                var position = UnityEngine.Random.insideUnitCircle * 4f;
                position.y *= 0.75f;

                ref var positionComponent = ref world.GetComponent<PositionComponent>(playerCharacterEntity);
                positionComponent.value = position;

                // set in position, configure controllable, etc
            }
        }
    }
}
