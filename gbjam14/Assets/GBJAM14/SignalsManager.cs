using Gemserk.Utilities.Signals;
using UnityEngine;

namespace GBJAM14
{
    public class SignalsManager : MonoBehaviour
    {
        public static SignalsManager instance;
        
        public SignalAsset onRoomInitSignal;
        public SignalAsset onExitRoomSignal;

        private void Awake()
        {
            instance = this;
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}