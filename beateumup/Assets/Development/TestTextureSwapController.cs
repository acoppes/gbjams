using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTextureSwapController : MonoBehaviour
{
    public SpriteRenderer spriteRendererA;
    public SpriteRenderer spriteRendererB;
    
    // Start is called before the first frame update
    void Start()
    {
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
            var color = colors[i];
            if (color.a > 0)
            {
                // rgb(64, 153, 255)
                colors[i] = new Color(0, 1, 0, 1);
            }
        }
        
        newTexture.SetPixels(colors);
        newTexture.Apply();

        var newSprite = Sprite.Create(newTexture, sprite.textureRect, new Vector2(sprite.pivot.x / texture.width, 
                sprite.pivot.y / texture.height), sprite.pixelsPerUnit, 
            0, SpriteMeshType.FullRect);

        spriteRendererB.sprite = newSprite;
    }
}
