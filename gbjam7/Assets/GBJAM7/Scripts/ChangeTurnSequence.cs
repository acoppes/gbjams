using System;
using UnityEngine;
using UnityEngine.UI;

namespace GBJAM7.Scripts
{
    public class ChangeTurnSequence : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private Text playerText;

        [SerializeField]
        private Text turnText;
        
        [NonSerialized]
        public bool completed;

        public void Show(PlayerData playerData, int player, int turn)
        {
            // set player and turn data
            gameObject.SetActive(true);
            
//            playerText.text = $"Player {player+1}";
            playerText.text = $"{playerData.name}";
            turnText.text = $"{turn + 1:00}";
            
            completed = false;
        }

        public void OnCompleted()
        {
            completed = true;
            gameObject.SetActive(false);
        }
        
        public void ForceComplete()
        {
            completed = true;
            gameObject.SetActive(false);
        }

    }
}
