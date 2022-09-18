using UnityEngine;

namespace GBJAM10
{
    [CreateAssetMenu(menuName = "GBJAM9/Weapon", fileName = "WeaponData", order = 0)]
    public class WeaponData : ScriptableObject
    {
        // for the animation
        public string attackType;
        public int damage;
        public float cooldown;
        public GameObject projectilePrefab;
    }
}