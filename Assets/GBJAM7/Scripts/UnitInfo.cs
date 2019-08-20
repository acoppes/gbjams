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
        private Text movementsLeftText;
        
        [SerializeField]
        private Text actionsLeftText;
        
        [SerializeField]
        private GameObject moneyContainer;
        
        public void Preview(Unit unit)
        {
            canvasGroup.alpha = 1;
            nameText.text = $"{unit.name}";
            hpText.text = $"{Mathf.RoundToInt(10 * unit.currentHP / unit.totalHP)}";
            dmgText.text = $"{unit.dmg}";
            playerText.text = $"P{unit.player + 1}";

            movementsLeftText.text = $"M:{unit.currentMovements}";
            actionsLeftText.text = $"A:{unit.currentActions}";
            
            moneyContainer.SetActive(unit.resources > 0);
            moneyContainer.GetComponentInChildren<Text>().text = $"{unit.resources}";
        }

        public void Hide()
        {
            canvasGroup.alpha = 0;
        }
    }
}
