using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTextureSwapController : MonoBehaviour
{
    public SpriteRenderer spriteRendererA;
    public SpriteRenderer spriteRendererB;

    public Texture2D paletteSwap;

    public int swapIndex;

    private Color[] colorMap;
    
    private Color GetColorSwap(Color color)
    {
        var width = paletteSwap.width;

        for (int i = 0; i < width; i++)
        {
            var textureColor = colorMap[i];
            var c0 = new Vector4(textureColor.r, textureColor.g, textureColor.b, textureColor.a);
            var c1 = new Vector4(color.r, color.g, color.b, color.a);
            
            if (Vector4.Distance(c0, c1) < 0.01f)
            {
                return colorMap[i + (swapIndex * width)];
            }
        }
        
        return color;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        colorMap = paletteSwap.GetPixels();
        
        var sprite = spriteRendererA.sprite;
        
        
        var texture = sprite.texture;
        var newTexture = new Texture2D(texture.width, texture.height, texture.format, false, false)
        {
            filterMode = texture.filterMode
        };

        Graphics.CopyTexture(texture, newTexture);

        var colors = newTexture.GetPixels();

        for (var i = 0; i < colors.Length; i++)
        {
            colors[i] = GetColorSwap(colors[i]);
        }
        
        newTexture.SetPixels(colors);
        newTexture.Apply();

        var newSprite = Sprite.Create(newTexture, sprite.textureRect, new Vector2(sprite.pivot.x / texture.width, 
                sprite.pivot.y / texture.height), sprite.pixelsPerUnit, 
            0, SpriteMeshType.FullRect);

        spriteRendererB.sprite = newSprite;
    }
}
