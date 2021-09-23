using UnityEngine;

namespace GBJAM9.Components
{
    public class ProjectileComponent : MonoBehaviour, IGameComponent
    {
        public int totalTargets = 1;
        
        public int damage;
        
        public GameObject hitSfxPrefab;
    }
}