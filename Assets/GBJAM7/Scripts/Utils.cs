using System.Linq;
using UnityEngine;

namespace GBJAM7.Scripts
{
    public class Utils
    {
        public static int GetDistance(Unit a, Unit b)
        {
            return GetDistance(a.transform.position, b.transform.position);
        }

        public static int GetDistance(Vector2 a, Vector2 b)
        {
            return Mathf.RoundToInt(Mathf.Abs(a.x - b.x) +
                                    Mathf.Abs(a.y - b.y));
        }

        public static bool IsInDistance(Vector2 a, Vector2 b, int distance)
        {
            return GetDistance(a, b) <= distance;
        }
        
        public static void UpdateEnemiesInRange()
        {
            // update all units, calculating enemies in range...

            var units = GameObject.FindObjectsOfType<Unit>().ToList();

            foreach (var unit in units)
            {
                unit.enemiesInRange = 0;
                
                if (unit.totalActions == 0)
                    continue;
                
                unit.enemiesInRange = units.Count(u =>
                {
                    if (u == unit || u.player == unit.player)
                        return false;
                    return GetDistance(unit, u) <= unit.attackDistance;
                });
            }
            
        }
    }
}