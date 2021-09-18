using GBJAM.Commons;
using UnityEngine;

namespace GBJAM7.Scripts
{
    public class PaletteSwapperSingleton : MonoBehaviour
    {
        [SerializeField]
        private PaletteSelectionAsset paletteSelectionAsset;

        private KeyCode[] paletteSwapKeys =
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.Alpha0
        };
        
        private void OnValidate()
        {
            gameObject.name = "~PaletteSwapperSingleton";
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            for (var i = 0; i < paletteSelectionAsset.palettes.Length; i++)
            {
                if (Input.GetKeyUp(paletteSwapKeys[i]))
                {
                    paletteSelectionAsset.currentPalette = i;
                }
            }
        }
    }
}
