Shader "Cathoder/Simple"
{
	Properties
	{
		_Intensity("Intensity", Range(0.1, 10)) = 3

		[Header(Textures)]
		_MainTex("Display", 2D) = "white" {}

		[NoScaleOffset]
		_ShadowMask("Shadow Mask", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
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
				float2 uv2 : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _ShadowMask;

			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float4 _ShadowMask_TexelSize;

			float  _Intensity;

			// Vertex Shader (same as Unity's default minus fog calculation).
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = o.uv * (_ShadowMask_TexelSize.xy * _MainTex_TexelSize.zw / _ShadowMask_TexelSize.xy);
				return o;
			}

			// Fragment Shader
			fixed4 frag(v2f i) : SV_Target
			{
				return tex2D(_MainTex, i.uv) * tex2D(_ShadowMask, i.uv2) *_Intensity;
			}
			ENDCG
	    }
	}
}
