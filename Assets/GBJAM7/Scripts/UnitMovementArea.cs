using System.Collections.Generic;
using UnityEngine;

namespace GBJAM7.Scripts
{
    public class UnitMovementArea : MonoBehaviour
    {
        [SerializeField]
        private GameObject areaPrefab;

        [SerializeField]
        private Transform areaContainer;
        
        public void Show(Unit unit)
        {
            var d = unit.movementDistance;
            
            var p = new Vector2Int(-d, -d);
            for (var i = p.x; i <= d; i++)
            {
                for (var j = p.y; j <= d; j++)
                {
                    var totalDistance = Mathf.Abs(i) + Mathf.Abs(j);
                    if (totalDistance <= d)
                    {
                        var offset = new Vector3(i * 1, j * 1, 0);
                        Instantiate(areaPrefab, unit.transform.position + offset, Quaternion.identity, areaContainer);
                    }
                }
            }
        }

        public void Hide()
        {
            var areas = new List<Transform>();
            for (var i = 0; i < areaContainer.childCount; i++)
            {
                var area = areaContainer.GetChild(i);
                areas.Add(area);
            }
            
            areaContainer.DetachChildren();
            areas.ForEach(a => Destroy(a.gameObject));
        }
    }
}