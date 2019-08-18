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
            // TODO: instantiate areas in container, use unit movement distance to spawn
            Instantiate(areaPrefab, unit.transform.position, Quaternion.identity, areaContainer);
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