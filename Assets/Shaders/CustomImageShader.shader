// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/gradientShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_PaletteTex("Texture", 2D) = "white" {}
		//_DisplaceTex("Displacement Texture", 2D) = "white" {}
		//_Magnitude("Magnitude", Range(0,0.1)) = 1
		out1("Output Colour 1", Color) = (1, 1, 1, 1)
		out2("Output Colour 2", Color) = (1, 1, 1, 1)
		out3("Output Colour 3", Color) = (1, 1, 1, 1)
		out4("Output Colour 4", Color) = (1, 1, 1, 1)
		//out5("Output Colour 5", Color) = (1, 1, 1, 1)
		_PaletteChoice("Choice", Range(0, 1)) = 0.5
		in1("Input Colour 1", Color) = (1, 1, 1, 1)
		in2("Input Colour 2", Color) = (1, 1, 1, 1)
		in3("Input Colour 3", Color) = (1, 1, 1, 1)
		in4("Input Colour 4", Color) = (1, 1, 1, 1)
		//in5("Input Colour 5", Color) = (1, 1, 1, 1)
		//out1("Output Colour 1", Color) = (1, 1, 1, 1)
	}
		SubShader
	{
		Tags {
		"RenderType" = "Opaque"
		//"Queue" = "Transparent"

		}
		LOD 100

		Pass
		{
		//Blend SrcAlpha OneMinusSrcAlpha

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
		sampler2D _PaletteTex;
		float _Magnitude;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			return o;
		}

		float _PaletteChoice;

		float4 in1;// = (0,0,0,1);
		float4 in2;// = (76.5, 76.5, 76.5, 1);
		float4 in3;// = (127.5, 127.5, 127.5, 1);
		float4 in4;// = (178.5, 178.5, 178.5, 1);
		//float4 in4; = (0, 0, 0, 1);
		//float4 in5;
		float4 out1;
		float4 out2;
		float4 out3;
		float4 out4;
		//float4 out5;

		float4 frag(v2f i) : SV_Target
		{
			float2 out1Pos = (_PaletteChoice, 0);
			out1 = tex2D(_PaletteTex, float2(0, _PaletteChoice));

			float2 out2Pos = (0, 0.5);
			out2 = tex2D(_PaletteTex, float2(0.26, _PaletteChoice));

			float2 out3Pos = (0, float2(0.51, _PaletteChoice));
			out3 = tex2D(_PaletteTex, out3Pos);

			float2 out4Pos = (0, float2(0.76, _PaletteChoice));
			out4 = tex2D(_PaletteTex, out4Pos);

			//float2 out5Pos = (0, 0.5);
			//out5 = tex2D(_PaletteTex, out5Pos);

			float4 color = tex2D(_MainTex, i.uv);

			half3 delta = abs(color.rgb - in1.rgb);
			color = (delta.r + delta.g + delta.b) < 0.01 ? out1 : color;
			if (all(color.rgb == out1.rgb)) { return color; }
			
			delta = abs(color.rgb - in2.rgb);
			color = (delta.r + delta.g + delta.b) < 0.01 ? out2 : color;
			if (all(color == out2)) { return color; }
			
			delta = abs(color.rgb - in3.rgb);
			color = (delta.r + delta.g + delta.b) < 0.01 ? out3 : color;
			if (all(color == out3)) { return color; }
			
			delta = abs(color.rgb - in4.rgb);
			color = (delta.r + delta.g + delta.b) < 0.05 ? out4 : color;
			if (all(color == out4)) { return color; }

			//delta = abs(color.rgb - in5.rgb);
			//color = (delta.r + delta.g + delta.b) < 0.01 ? out5 : color;

			return color;
		}
		ENDCG
	}
	}
}
