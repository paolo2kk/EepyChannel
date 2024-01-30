Shader "Hidden/StaticTexture"
{
		SubShader
	{
		Lighting Off

		Pass
		{
			CGPROGRAM
			#include "UnityCustomRenderTexture.cginc"

			#pragma vertex InitCustomRenderTextureVertexShader
			#pragma fragment frag

			float4 frag(v2f_init_customrendertexture IN) : COLOR
			{
				float random = frac(sin(dot(float3(_Time.x, IN.texcoord.x, IN.texcoord.y), float3(40.3278, 20.345, 78.0987))) * 10000);
				return float4(random, random, random, 1);
			}
			ENDCG
		}
	}
}