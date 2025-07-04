Shader "Unlit/FogDisplayShader"
{
    Properties
    {
        _FogColor("Fog Color", Color) = (0.05, 0.1, 0.2, 1.0)
        _FogMask("Fog Mask", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent-10" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

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
                float2 worldPos : TEXCOORD1;
            };

            sampler2D _FogMask;
            fixed4 _FogColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float4 world = mul(unity_ObjectToWorld, v.vertex);
                o.worldPos = world.xy;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the fog mask (white = revealed, black = fog)
                float reveal = tex2D(_FogMask, i.uv).r;
                float alpha = 1.0 - reveal; // invert reveal mask to fog alpha

                return _FogColor * alpha;
            }
            ENDCG
        }
    }
}
