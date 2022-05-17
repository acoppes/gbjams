using UnityEngine;

namespace GBJAM9.Ecs
{
    public struct PositionComponent
    {
        public Vector2 value;
    }
    
    public struct UnitInputComponent
    {
        public Vector2 movementDirection;
        
        public Vector2 attackDirection;

        public bool attack;

        public bool dash;
    }

    public struct UnitModelComponent
    {
        public GameObject prefab;
        public GameObject instance;
    }
}