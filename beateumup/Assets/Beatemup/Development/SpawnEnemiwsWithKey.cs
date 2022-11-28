using Beatemup;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnEnemiwsWithKey : MonoBehaviour
{
    public GameObject enemyDefinition;
    public InputAction spawnEnemyAction;

    public BoxCollider2D spawnArea;

    private void OnEnable()
    {
        spawnEnemyAction.Enable();
    }

    void Update()
    {
        if (spawnEnemyAction.WasReleasedThisFrame())
        {
            var enemyEntity = World.Instance.CreateEntity(enemyDefinition.GetInterface<IEntityDefinition>());
            ref var enemyPosition = ref World.Instance.GetComponent<PositionComponent>(enemyEntity);

            enemyPosition.value = new Vector3(UnityEngine.Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x), 
                UnityEngine.Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y), 0);
            
            ref var player = ref World.Instance.GetComponent<PlayerComponent>(enemyEntity);
            // player.player = enemyPosition.value.x < 0 ? 0 : 1;
            player.player = 1;
        }
    }
}
