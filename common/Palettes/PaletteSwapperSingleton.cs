using GBJAM.Commons;
using UnityEngine;

namespace GBJAM7.Scripts
{
    public class PaletteSwapperSingleton : MonoBehaviour
    {
        [SerializeField]
        private PaletteSelectionAsset paletteSelectionAsset;
        
        [SerializeField]
        private GameObject paletteCameraPrefab;
        
        private PaletteSwap paletteSwap;

        [SerializeField]
        private KeyCode nextPaletteKeyCode;
        
        [SerializeField]
        private KeyCode previousPaletteKeyCode;

        private void OnValidate()
        {
            gameObject.name = "~PaletteSwapperSingleton";
        }

        private void CreateCamera()
        {
            var cameraInstance = GameObject.Instantiate(paletteCameraPrefab);
            cameraInstance.name = "~PaletteCamera";
            GameObject.DontDestroyOnLoad(cameraInstance);
            paletteSwap = cameraInstance.GetComponent<PaletteSwap>();
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (paletteCameraPrefab == null)
            {
                return;
            }
            
            var nextPalette = Input.GetKeyUp(nextPaletteKeyCode);
            var previousPalette = Input.GetKeyUp(previousPaletteKeyCode);

            if (paletteSwap == null &&  (nextPalette || previousPalette))
            {
                CreateCamera();
            }

            if (nextPalette)
            {
                paletteSwap.NextPalette();
            }

            if (previousPalette)
            {
                paletteSwap.PreviousPalette();
            }
        }
    }
}
