using UnityEngine;

namespace GBJAM9
{
    [CreateAssetMenu(menuName = "GBJAM9/Weapon", fileName = "WeaponData", order = 0)]
    public class WeaponData : ScriptableObject
    {
        // for the animation
        public string attackType;
        public GameObject projectilePrefab;
    }
}