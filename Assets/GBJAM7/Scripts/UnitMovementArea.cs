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
        
        public void Show(Vector3 position, int minDistance, int maxDistance)
        {
            if (minDistance == 0 && maxDistance == 0)
                return;
            
            var p = new Vector2Int(-maxDistance, -maxDistance);
            for (var i = p.x; i <= maxDistance; i++)
            {
                for (var j = p.y; j <= maxDistance; j++)
                {
                    var totalDistance = Mathf.Abs(i) + Mathf.Abs(j);
                    if (totalDistance <= maxDistance && totalDistance >= minDistance)
                    {
                        var offset = new Vector3(i * 1, j * 1, 0);
                        Instantiate(areaPrefab, position + offset, Quaternion.identity, areaContainer);
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