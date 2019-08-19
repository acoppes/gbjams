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
        private Text movementsText;
        
        [SerializeField]
        private GameObject moneyContainer;
        
        public void Preview(Unit unit)
        {
            canvasGroup.alpha = 1;
            nameText.text = $"{unit.name}";
            hpText.text = $"{unit.hp}";
            dmgText.text = $"{unit.dmg}";
            movementsText.text = $"{unit.currentMovements}";

            moneyContainer.SetActive(unit.resources > 0);
            moneyContainer.GetComponentInChildren<Text>().text = $"{unit.resources}";
        }

        public void Hide()
        {
            canvasGroup.alpha = 0;
        }
    }
}
