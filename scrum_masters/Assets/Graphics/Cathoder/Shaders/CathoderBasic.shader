Shader "Cathoder/Basic"
{
	Properties
	{
		_Intensity("Intensity",  Range(0.1, 10)) = 3

		// Textures
		[Header(Textures)]
		
		_MainTex("Display", 2D) = "white" {}

		[NoScaleOffset]
		_ShadowMask("Shadow Mask", 2D) = "white" {}

		// Color Bleeding / Composite
		[Header(Color Bleed)]
	
		_Bleed("Bleed",          Range(0, 2))    = 0.4
		_Composite("Composite",  Range(-2, 2))   = 0

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
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _ShadowMask;

			float _Intensity;
			float _Bleed;
			float _Composite;

			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float4 _ShadowMask_TexelSize;

			// Vertex Shader (same as Unity's default minus fog calculation).
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			// Fragment Shader
			fixed4 frag(v2f i) : SV_Target
			{
				// Shadow Mask
				fixed4 shadow = tex2D(_ShadowMask, i.uv * (_ShadowMask_TexelSize.xy / _MainTex_TexelSize.xy / _ShadowMask_TexelSize.xy));

				// Main texture
				fixed4 tex = tex2D(_MainTex, i.uv);

				// Color Bleed
				fixed4 bleed = tex2D(_MainTex, i.uv + float2(_MainTex_TexelSize.x * _Composite, _MainTex_TexelSize.y * -_Composite));

				// Final Result
				return (tex + bleed * _Bleed) * shadow *_Intensity;
			}
			ENDCG
	    }
	}
}
