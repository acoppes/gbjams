using System;
using UnityEngine;

namespace GBJAM.Commons.Palettes
{
    [ExecuteInEditMode]
    public class PaletteSwap : MonoBehaviour
    {
        // public PaletteSelectionAsset paletteSelection;

        public Texture2D lutTexture;
        public Shader shader;
    
        private Material _mat;

        // [NonSerialized]
        // public int currentPalette;

        // public void NextPalette()
        // {
        //     currentPalette++;
        //     if (currentPalette >= paletteSelection.palettes.Length)
        //     {
        //         currentPalette = 0;
        //     }
        // }
        //
        // public void PreviousPalette()
        // {
        //     currentPalette--;
        //     if (currentPalette < 0)
        //     {
        //         currentPalette = paletteSelection.palettes.Length - 1;
        //     }
        // }
        
        private void OnEnable()
        {
            if (_mat != null)
            {
                DestroyImmediate(_mat);
                _mat = null;
            }
            
            if (shader != null)
            {
                _mat = new Material(shader);
            }

            // currentPalette = paletteSelection.defaultPalette;
        }

        private void OnDisable()
        {
            if (_mat != null)
            {
                DestroyImmediate(_mat);
                _mat = null;
            }
        }
        

        private void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            if (_mat == null || shader == null)
                return;

            // var palette = paletteSelection.palettes[currentPalette];
            _mat.SetTexture("_PaletteTex", lutTexture);
            Graphics.Blit(src, dst,  _mat);
        }
    }
}