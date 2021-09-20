using UnityEngine;

namespace GBJAM.Commons
{
    public class GameboyButtonsUpdater : MonoBehaviour
    {
        public GameboyButtonKeyMapAsset gameboyKeyMap;

        private void Update()
        {
            gameboyKeyMap.UpdateControlState();
        }
    }
}