using GBJAM7.Scripts.MainMenu;
using UnityEngine;

[ExecuteInEditMode]
public class PaletteSwap : MonoBehaviour
{
    public PaletteSelectionAsset paletteSelection;

    public Shader shader;
    
    private Material _mat;

    private void OnEnable()
    {
        if (_mat == null)
            _mat = new Material(shader);
    }

    private void OnDisable()
    {
        if (_mat != null)
            DestroyImmediate(_mat);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (paletteSelection == null)
            return;
        _mat.SetTexture("_PaletteTex", paletteSelection.GetCurrentPalette());
        Graphics.Blit(src, dst,  _mat);
    }
}