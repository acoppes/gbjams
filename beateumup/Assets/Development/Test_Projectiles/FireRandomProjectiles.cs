using Beatemup;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;
using LookingDirection = Beatemup.Ecs.LookingDirection;

public class FireRandomProjectiles : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject parametersObject;

    public World world;
    
    public void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            var projectile = world.CreateEntity(projectilePrefab.GetInterface<IEntityDefinition>(),
                parametersObject.GetComponentsInChildren<IEntityInstanceParameter>());

            if (world.HasComponent<LookingDirection>(projectile))
            {
                var direction = Vector2.right.Rotate(UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad);

                ref var lookingDirection = ref world.GetComponent<LookingDirection>(projectile);
                lookingDirection.value = new Vector3(direction.x, UnityEngine.Random.Range(0.15f, 0.4f), direction.y);
            }

            if (world.HasComponent<PositionComponent>(projectile))
            {
                var mousePosition = Mouse.current.position.ReadValue();
                var position = Camera.main.ScreenToWorldPoint(mousePosition);

                ref var positionComponent = ref world.GetComponent<PositionComponent>(projectile);
                positionComponent.value = new Vector3(position.x, 0,
                    position.y);
            }
        }
    }
}
