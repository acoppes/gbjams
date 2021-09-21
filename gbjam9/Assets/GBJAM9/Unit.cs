using UnityEngine;

namespace GBJAM9
{
    public class Unit : MonoBehaviour
    {
        public int player;

        private UnitSystem unitSystem;
        
        private void Awake()
        {
            unitSystem = FindObjectOfType<UnitSystem>();
        }

        private void OnEnable()
        {
            if (unitSystem != null)
            {
                unitSystem.units.Add(this);
            }
        }

        private void OnDisable()
        {
            if (unitSystem != null)
            {
                unitSystem.units.Remove(this);
            }
        }
    }
}