// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Sprites/Unlit Color Adjust"
{
    // source from https://forum.unity.com/threads/hue-saturation-brightness-contrast-shader.260649/
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Hue ("Hue", Range(-0.5, 0.5)) = 0.
        _Brightness ("Brightness", Range(-1, 1)) = 0.
        _Contrast("Contrast", Range(0, 2)) = 1
        _Saturation("Saturation", Range(0, 2)) = 1
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

            float _Hue;
            float _Brightness;
            float _Contrast;
            float _Saturation;

            inline float3 applyHue(float3 aColor, float aHue)
            {
                float angle = radians(aHue);
                float3 k = float3(0.57735, 0.57735, 0.57735);
                float cosAngle = cos(angle);
                //Rodrigues' rotation formula
                return aColor * cosAngle + cross(k, aColor) * sin(angle) + k * dot(k, aColor) * (1 - cosAngle);
            }
            inline float4 applyHSBEffect(float4 startColor)
            {
                float4 outputColor = startColor;
                outputColor.rgb = applyHue(outputColor.rgb, _Hue * 360.0f);
                outputColor.rgb = (outputColor.rgb - 0.5f) * (_Contrast)+0.5f;
                outputColor.rgb = outputColor.rgb + _Brightness;
                float3 intensity = dot(outputColor.rgb, float3(0.299, 0.587, 0.114));
                outputColor.rgb = lerp(intensity, outputColor.rgb, _Saturation);
                return outputColor;
            }
            
            fixed4 SpriteFragWithLut(v2f IN) : SV_Target
            {
                float4 startColor = SampleSpriteTexture (IN.texcoord);

                // float4 lutc = c * IN.color;

                // float4 startColor = tex2D(_MainTex, i.uv);
                float4 hsbColor = applyHSBEffect(startColor);
              
                
                // float4 lutc = tex2D(_LutTex, fixed2(c.r, 0)) * IN.color;
                // lutc.rgb = lerp(lutc.rgb, _FlashColor.rgb, _FlashAmount);
                
                hsbColor.rgb *= startColor.a;
                hsbColor.a = startColor.a;
                
//                return lutc;
                return hsbColor;
            }
        
        ENDCG
        }
    }
}