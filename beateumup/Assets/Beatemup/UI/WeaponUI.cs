using System.Collections.Generic;
using UnityEngine;

namespace Beatemup.UI
{
    public class WeaponUI : MonoBehaviour
    {
        public List<GameObject> weaponIcons;

        public void UpdateWeapon(int weapon)
        {
            for (int i = 0; i < weaponIcons.Count; i++)
            {
                weaponIcons[i].SetActive(i == weapon);
            }
        }
    }
}