using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace GBJAM13.Components
{
    public struct MapElementComponent
    {
        public string type;
        public string element;
        public string name;
        
        public bool mainPath;

        public int column;
        public int row;

        public bool outsideTravelPath;
        
        public Vector3 shipOffset;
    }
    
    public struct MapShipNodeComponent
    {
        
    }
}