using UnityEngine;

namespace GBJAM9.Components
{
    public class AttackComponent : MonoBehaviour, IGameComponent
    {
        public WeaponData weaponData;
        public Transform attackAttachPoint;
    }
}