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
        
        public void Show(Vector3 position, int distance)
        {
            var p = new Vector2Int(-distance, -distance);
            for (var i = p.x; i <= distance; i++)
            {
                for (var j = p.y; j <= distance; j++)
                {
                    var totalDistance = Mathf.Abs(i) + Mathf.Abs(j);
                    if (totalDistance <= distance)
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