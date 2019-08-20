using UnityEngine;
using UnityEngine.UI;

namespace GBJAM7.Scripts
{
    public class UnitInfo : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private Text nameText;
        
        [SerializeField]
        private Text hpText;
        
        [SerializeField]
        private Text dmgText;
        
        [SerializeField]
        private Text playerText;
        
        [SerializeField]
        private GameObject moneyContainer;
        
        public void Preview(Unit unit)
        {
            canvasGroup.alpha = 1;
            nameText.text = $"{unit.name}";
            hpText.text = $"{unit.hp}";
            dmgText.text = $"{unit.dmg}";
            playerText.text = $"P{unit.player + 1}";

            moneyContainer.SetActive(unit.resources > 0);
            moneyContainer.GetComponentInChildren<Text>().text = $"{unit.resources}";
        }

        public void Hide()
        {
            canvasGroup.alpha = 0;
        }
    }
}
