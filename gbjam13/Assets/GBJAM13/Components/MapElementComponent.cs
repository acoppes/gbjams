using Gemserk.Leopotam.Ecs;

namespace GBJAM13.Components
{
    public struct MapElementComponent
    {
        public string type;
        public string element;
        public bool mainPath;
    }
    
    public struct MapDestinationComponent
    {
        
    }
    
    public struct SelectedComponent
    {
        
    }
    
    public struct HasSelectionIndicator
    {
        public Entity selection;
    }
}