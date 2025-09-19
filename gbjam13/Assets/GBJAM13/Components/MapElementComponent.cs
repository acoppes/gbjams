using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace GBJAM13.Components
{
    public struct MapElementComponent
    {
        public string type;
        public string element;
        public bool mainPath;

        public Vector3 shipOffset;
    }
    
    public struct MapShipNodeComponent
    {
        
    }
    
    public struct MapDestinationComponent
    {
        
    }
    
    public struct SelectedComponent
    {
        
    }
    
    public struct HasSelectionIndicatorComponent
    {
        public Entity selection;
    }
}