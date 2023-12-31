Shader "Hidden/Shader/VHS_Tape_Rewind"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		HLSLINCLUDE

	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	TEXTURE2D_X(_MainTex);
	SAMPLER(sampler_MainTex);


	CBUFFER_START(UnityPerMaterial)
		half4 _MainTex_TexelSize;
	CBUFFER_END

	half fade;
	half intencity;

	struct Attributes
	{
		float4 positionOS       : POSITION;
		float2 uv               : TEXCOORD0;
	};

	struct Varyings
	{
		float2 uv        : TEXCOORD0;
		float4 vertex : SV_POSITION;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	Varyings vert(Attributes input)
	{
		Varyings output = (Varyings)0;
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

		VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
		output.vertex = vertexInput.positionCS;
		output.uv = input.uv;

		return output;
	}

	half4 Frag(Varyings i) : COLOR
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

		float2 uv = i.uv;
		float2 displacementSampleUV = float2(uv.x + (_Time.y+20)*70, uv.y);

		float da = intencity;

		float displacement = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, displacementSampleUV).x * da;

		float2 displacementDirection = float2(cos(displacement * 6.28318530718), sin(displacement * 6.28318530718));
		float2 displacedUV = (uv + displacementDirection * displacement);
		float4 shade = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, displacedUV);
		float4 main = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
		return float4(lerp(main,shade,fade));
	}
		ENDHLSL

		Subshader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			 HLSLPROGRAM
			 #pragma fragmentoption ARB_precision_hint_fastest
			 #pragma vertex vert
			 #pragma fragment Frag
			 ENDHLSL
		}


	}
	Fallback off
}

