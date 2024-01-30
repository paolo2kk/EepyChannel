Shader "Cathoder/Standard"
{
	Properties
	{
		// Display Settings
		[KeywordEnum(Color, Monochrome, Amber, Lazy Green, Damaged, Negative)]
		_Display("Display Type", Float) = 0

		_RGBA("Color", Color) = (1, 1, 1, 1)

		_Intensity("Intensity",	Range(0.1, 10)) = 2

		[Toggle]
		_Binary("Binary Pixels",	Float) = 0

		[Toggle]
		_Lcd("LCD Mode (Pixel Snap)",	Float) = 0

		// Textures
		[Header(Display)]
		[Toggle]
		_SourceRes("Use Source Resolution",	Float) = 0

		[IntRange]
		_Width("Width",		    Range(8, 1920)) = 320

		[IntRange]
		_Height("Height",	    Range(8, 1080)) = 240
		_MainTex("Signal Input", 2D) = "white" {}

		[NoScaleOffset]
		_ShadowMask("Shadow Mask", 2D) = "white" {}

		// Color Bleed / Composite
		[Header(Composite Bleed)]
		[Toggle]
		_ColorBleed("Enable",	Float) = 1
		_Bleed("Intensity",		Range(0, 2)) = 0.75
		_Composite("Range",		Range(-2, 2)) = 1

		// Overscan
		[Header(Overscan)]
		[Toggle]
		_Overscan("Enable",		Float) = 1

		_OverscanX("X",			Range(0, 1)) = 0
		_OverscanY("Y",			Range(0, 1)) = 0
		_OverscanColor("Color", Color) = (0, 0, 0, 1)

		[Header(Picture in Picture)]
		[Toggle]
		_PiP("Enable",		 Float) = 1
		_PiPAlpha("Opacity", Range(0, 1)) = 1
		[NoScaleOffset]
		_PiPTex("Signal Input", 2D) = "white" {}


		[Header(Overlay)]
		[Toggle]
		_Overlay("Enable",		   Float) = 0
		_OverlayAlpha("Opacity",   Range(0, 1)) = 1

		[NoScaleOffset]
		_OverlayTex("Overlay", 2D) = "black" {}
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

			// Ubershader handling
			#pragma multi_compile _DISPLAY_COLOR   _DISPLAY_MONOCHROME _DISPLAY_AMBER _DISPLAY_LAZY_GREEN _DISPLAY_DAMAGED _DISPLAY_NEGATIVE
			#pragma multi_compile _OVERSCAN_OFF    _OVERSCAN_ON
			#pragma multi_compile _COLORBLEED_OFF  _COLORBLEED_ON
			#pragma multi_compile _MULTIUV         _SINGLEUV
			#pragma multi_compile _SOURCERES_OFF   _SOURCERES_ON
			#pragma multi_compile _OVERLAY_OFF     _OVERLAY_ON
			#pragma multi_compile _PIP_OFF		   _PIP_ON
			#pragma multi_compile _LCD_OFF		   _LCD_ON
			#pragma multi_compile _BINARY_OFF	   _BINARY_ON

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv  : TEXCOORD0;
				float2 uv1 : TEXCOORD1;

				// Vertex UV handling
				float2 uv2 : TEXCOORD2;
				#if defined(_COLORBLEED_ON)
				float2 uv3 : TEXCOORD3;
				#endif

				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _ShadowMask;

			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float4 _ShadowMask_TexelSize;

			fixed4 _RGBA;

			#if defined(_PIP_ON)
			sampler2D _PiPTex;
			fixed _PiPAlpha;
			#endif

			#if defined(_OVERLAY_ON)
			sampler2D _OverlayTex;
			fixed _OverlayAlpha;
			#endif

			fixed _Intensity;
			fixed _Width;
			fixed _Height;

			#if defined(_COLORBLEED_ON)
			fixed _Bleed;
			fixed _Composite;
			#endif

			#if defined(_OVERSCAN_ON)
			fixed  _OverscanX;
			fixed  _OverscanY;
			fixed4 _OverscanColor;
			#endif

			// Vertex Shader (same as Unity's default minus fog calculation).
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv1 = v.uv;

				// Use the same resolution as the source image.
				#if defined(_SOURCERES_ON)
				o.uv2 = v.uv * (_ShadowMask_TexelSize.xy / _MainTex_TexelSize.xy / _ShadowMask_TexelSize.xy);

				// Use shader-defined resolution.
				#else
				o.uv2 = v.uv * (_ShadowMask_TexelSize.xy * float2(_Width, _Height) / _ShadowMask_TexelSize.xy);
				#endif

				// Color Bleeding
				#if defined(_COLORBLEED_ON)
				o.uv3 = o.uv + float2(_MainTex_TexelSize.x * (_Composite), _MainTex_TexelSize.y * -(_Composite));
				#endif

				return o;
			}

			// Fragment Shader
			fixed4 frag(v2f i) : SV_Target
			{
				#if defined(_LCD_ON)
				float2 size = float2(_Width, _Height);
				i.uv = i.uv*_MainTex_TexelSize.zw;
				i.uv = ceil(i.uv / (_MainTex_TexelSize.zw / size)) / size;
				#endif

				// Shadow Mask / Pixel Layout
				fixed4 shadow = tex2D(_ShadowMask, i.uv2);

				shadow *= _Intensity * _RGBA;

				// Overlay
				#if defined(_OVERLAY_ON)
				fixed4 overlay = tex2D(_OverlayTex, i.uv1);
				#endif

				// Display Color
				fixed4 tex = tex2D(_MainTex, i.uv);

				#if defined(_PIP_ON)
				fixed4 pip = tex2D(_PiPTex, i.uv1);
				tex.rgb    = lerp(tex.rgb, pip.rgb, pip.a*_PiPAlpha);
				#endif

			

				// Color Bleeding
				#if defined(_COLORBLEED_ON)
					fixed4 bleed = tex2D(_MainTex, i.uv3);
				#endif

				#if defined(_BINARY_ON)
					tex = round(tex);
					#if defined(_COLORBLEED_ON)
					bleed = round(bleed);
					#endif
				#endif

				// Display: Monochrome
				#if defined(_DISPLAY_MONOCHROME)
					tex = tex.xxxw;

					// Monochrome, Color Bleed
					#if defined(_COLORBLEED_ON)
					bleed = bleed.xxxw;
					#endif

				// Display: Amber
				#elif defined(_DISPLAY_AMBER)
					tex = fixed4(tex.y*0.75, tex.y*0.5, 0, 1);

					// Amber, Color Bleed
					#if defined(_COLORBLEED_ON)
					bleed = fixed4(0.1 + bleed.y*0.5, bleed.y*0.25,  0, 1);
					#endif

				// Display: Green
				#elif defined(_DISPLAY_LAZY_GREEN)
					tex = fixed4(0, 0.1 + tex.y, 0, 1);

					// Green, Color Bleed
					#if defined(_COLORBLEED_ON)
					bleed = fixed4(0, bleed.y*0.5, 0, 1);
					#endif

				// Display: Broken
				#elif defined(_DISPLAY_DAMAGED)
					tex = fixed4(0.3, tex.y, tex.x*0.5, 1);
				#elif defined(_DISPLAY_NEGATIVE)
					tex.rgb   = 1 - tex.rgb;

					#if defined(_COLORBLEED_ON)
					bleed.rgb = 1 - bleed.rgb;
					#endif
				#endif

				#if defined(_COLORBLEED_ON)
					tex += bleed * _Bleed;
				#endif

				// Overscan
				#if defined(_OVERSCAN_ON)
					float2 uvfrac = frac(i.uv);
					if (uvfrac.x > 1 - _OverscanX || uvfrac.x < _OverscanX || uvfrac.y > 1 - _OverscanY || uvfrac.y < _OverscanY) 
						tex = _OverscanColor;
			
				#endif

				tex *= shadow;

				#if defined(_OVERLAY_ON)
				return fixed4(lerp(tex.rgb, overlay.rgb, _OverlayAlpha*overlay.a), 1);
				#endif

				return tex;
			}

			ENDCG
		}
	}
}
