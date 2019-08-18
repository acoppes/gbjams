using UnityEngine;
using UnityEngine.UI;

namespace GBJAM7.Scripts
{
    public class UnitInfo : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private Text hpText;
        
        [SerializeField]
        private Text dmgText;
        
        [SerializeField]
        private Text movementsText;
        
        public void Preview(Unit unit)
        {
            canvasGroup.alpha = 1;
            hpText.text = $"HP:{unit.hp}";
            dmgText.text = $"DMG:{unit.dmg}";
            movementsText.text = $"MOV LEFT:{unit.movementsLeft}";
        }

        public void Hide()
        {
            canvasGroup.alpha = 0;
        }
    }
}
