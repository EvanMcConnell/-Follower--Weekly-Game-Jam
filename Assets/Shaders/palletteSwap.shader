// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/palletteSwap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

			sampler2D _MainTex;

			half4 _In0;
			half4 _Out0;
			half4 _In1;
			half4 _Out1;
			half4 _In2;
			half4 _Out2;
			half4 _In3;
			half4 _Out3;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                half4 col = tex2D(_MainTex, i.uv);
                
				if (all(col.rgb == _In0.rgb))
					return _Out0;

				if (all(col.rgb == _In1.rgb))
					return _Out1;

				if (all(col.rgb == _In2.rgb))
					return _Out2;

				if (all(col.rgb == _In3.rgb))
					return _Out3;

                return col;
            }
            ENDCG
        }
    }
}
