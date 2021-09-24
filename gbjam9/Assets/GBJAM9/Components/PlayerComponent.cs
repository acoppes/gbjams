using UnityEngine;

namespace GBJAM9.Components
{
    public class PlayerComponent : MonoBehaviour, IGameComponent
    {
        public int player;

        public int layer;
        public int enemyLayer;
        public int projectileLayer;

        public int layerMask;
        public int enemyLayerMask;
    }
}