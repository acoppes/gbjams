using UnityEngine;
using UnityEngine.UI;

namespace GBJAM7.Scripts
{
    public class GameInfo : MonoBehaviour
    {
        [SerializeField]
        private Text playerText;
        [SerializeField]
        private Text turnText;

        public void UpdateGameInfo(int player, int turn)
        {
            playerText.text = $"P{player + 1}";
            turnText.text = $"{turn + 1}";
        }
    }
}
