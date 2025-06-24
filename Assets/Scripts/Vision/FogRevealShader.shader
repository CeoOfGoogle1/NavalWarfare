Shader "Unlit/FogRevealShader"
{
    Properties
    {
        _Position("Position", Vector) = (0,0,0,0)
        _RevealRadius("Reveal Radius", Float) = 5.0
        _FadeAmount("Fade Amount", Float) = 3.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZWrite Off
            Cull Off
            Blend One One // additive blending for overlapping vision areas

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Position;
            float _RevealRadius;
            float _FadeAmount;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dist = distance(i.uv, _Position.xy);
                float alpha = 0.0 + smoothstep(_RevealRadius, _RevealRadius - _FadeAmount, dist);
                return fixed4(alpha, alpha, alpha, alpha);
            }
            ENDCG
        }
    }
}
