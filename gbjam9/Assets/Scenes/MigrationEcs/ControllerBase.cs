using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public abstract class ControllerBase : MonoBehaviour, IController
{
    protected Gemserk.Leopotam.Ecs.World world;
    protected Entity entity;

    public void Bind(Gemserk.Leopotam.Ecs.World world, Entity entity)
    {
        this.world = world;
        this.entity = entity;
    }

    public abstract void OnUpdate(float dt);
}