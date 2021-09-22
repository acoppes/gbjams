using UnityEngine;

namespace GBJAM9.Components
{
    public class UnitComponent : MonoBehaviour, IGameComponent
    {
        public int player;

        private EntityManager entityManager;
        
        private void Awake()
        {
            entityManager = FindObjectOfType<EntityManager>();
        }

        private void OnEnable()
        {
            if (entityManager != null)
            {
                entityManager.units.Add(this);
            }
        }

        private void OnDisable()
        {
            if (entityManager != null)
            {
                entityManager.units.Remove(this);
            }
        }
    }
}