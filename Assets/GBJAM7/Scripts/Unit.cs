using UnityEngine;

namespace GBJAM7.Scripts
{
    public class Unit : MonoBehaviour
    {
        public enum UnitType
        {
            Unit,
            Spawner
        }
        
        public string name;

        public UnitType unitType;
        
        public int movementDistance;

        public int totalMovements = 1;
        public int currentMovements = 1;
        
        public int hp;
        public int dmg;

        public int spawnUnitsLeft = 1;
    }
}
