Shader "Unlit/SpriteAlfa"
{
     Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _FadeRadius ("Fade Radius", Range(0, 1)) = 0.5
        _FadeSoftness ("Fade Softness", Range(0, 1)) = 0.2
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _FadeRadius;
            float _FadeSoftness;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                
                // Переводим UV в координаты от центра (-0.5 до 0.5)
                float2 centerUV = v.uv - 0.5;
                o.worldPos = centerUV;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                
                // Расстояние от центра (0-0.5)
                float distanceFromCenter = length(i.worldPos) * 2.0; // Умножаем на 2, чтобы охватить весь спрайт
                
                // Вычисляем прозрачность
                float alpha = smoothstep(_FadeRadius + _FadeSoftness, _FadeRadius, distanceFromCenter);
                col.a *= alpha;
                
                return col;
            }
            ENDCG
        }
    }
}
