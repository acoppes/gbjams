using UnityEngine;

namespace GBJAM9.Components
{
    public class AttackComponent : MonoBehaviour, IGameComponent
    {
        public string attackType;
        public GameObject projectilePrefab;
    }
}