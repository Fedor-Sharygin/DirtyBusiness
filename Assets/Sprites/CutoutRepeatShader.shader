Shader "Unlit/CutoutRepeatShader"
{
    Properties
    {
        _TintColor("Tint Color", Color) = (0,0,0,1)
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}

        _Tiling("Default Tiling", Vector) = (1,1,0,0)
        _Scale("Current Scale", Vector) = (1,1,0,0)
        
        _Alpha("Alpha", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _TintColor;

            float4 _Tiling;
            float2 _Scale;
            float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = (TRANSFORM_TEX(v.uv, _MainTex) * _Tiling.xy + _Tiling.zw) * _Scale;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = (tex2D(_MainTex, i.uv) + _Color) * _TintColor;
                clip(col.a - 1.0);
                col.a = col.a * _Alpha;
                return col;
            }
            ENDCG
        }
    }
}
