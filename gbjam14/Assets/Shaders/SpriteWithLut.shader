// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Sprites/Sprite With Lut"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [PerRendererData] _LutTex ("Lut", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
        _FlashColor ("Flash Color", Color) = (1,1,1,1)
		_FlashAmount ("Flash Amount", Range (0,1)) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment SpriteFragWithLut
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

            sampler2D _LutTex;
            fixed4 _FlashColor;
            float _FlashAmount;
            
            fixed4 SpriteFragWithLut(v2f IN) : SV_Target
            {
                // float4 c = tex2D(_MainTex, IN.texcoord);
                float4 c = SampleSpriteTexture (IN.texcoord);
                // float x = c / 8;
                // float4 lutc = tex2D(_LutTex, c.xz) * IN.color;
                // lutc.rgb *= c.a;
                
                // float4 lutc = tex2D(_LutTex, IN.texcoord);
                float4 lutc = tex2D(_LutTex, fixed2(c.r, c.g)) * IN.color;

                lutc.rgb = lerp(lutc.rgb, _FlashColor.rgb, _FlashAmount);
                
                lutc.rgb *= c.a;
                lutc.a = c.a;
                return lutc;
            }
        
        ENDCG
        }
    }
}