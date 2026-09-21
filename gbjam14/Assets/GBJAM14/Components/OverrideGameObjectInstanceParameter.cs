using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using UnityEngine;

namespace GBJAM14.Components
{
    public class OverrideGameObjectInstanceParameter : MonoBehaviour, IEntityInstanceParameter
    {
        public GameObject gameObject;
        
        public void Apply(World world, Entity entity)
        {
            entity.Get<GameObjectComponent>().prefab = gameObject;
        }
    }
}