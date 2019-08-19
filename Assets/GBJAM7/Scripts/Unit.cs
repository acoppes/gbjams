using UnityEngine;

namespace GBJAM7.Scripts
{
    public class Unit : MonoBehaviour
    {
        public enum UnitType
        {
            Unit,
            Spawner,
            MainBase
        }

        public int player;
        
        public string name;

        public UnitType unitType;
        
        public int movementDistance;

        public int totalMovements = 1;
        public int currentMovements = 1;
        
        public int hp;
        public int dmg;

        public int totalActions = 1;
        public int currentActions = 1;

        public int resources;
    }
}
