Shader "Hidden/Shader/Glitch1" 
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		HLSLINCLUDE

	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	TEXTURE2D(_MainTex);
	SAMPLER(sampler_MainTex);
	TEXTURE2D(_Mask);
	SAMPLER(sampler_Mask);
	float _FadeMultiplier;
#pragma shader_feature ALPHA_CHANNEL

	uniform float T;
	uniform float Speed;
	uniform float Strength;
	uniform float Fade;
	float x = 127.1;
	float angleY = 311.7;
	float y = 43758.5453123;
	uniform float Stretch = 0.02;
	half mR = 0.08;
	half mG = 0.07;
	half mB = 0.0;


	float hash(float2 d)
	{
		float m = dot(d, float2(x, angleY));
		return -1.0 + 2.0 * frac(sin(m) * y);
	}
	float noise(float2 d)
	{
		float2 i = floor(d);
		float2 f = frac(d);
		float2 u = f * f * (3.0 - 2.0 * f); // 
		return lerp(lerp(hash(i + float2(0.0, 0.0)), hash(i + float2(1.0, 0.0)), u.x), lerp(hash(i + float2(0.0, 1.0)), hash(i + float2(1.0, 1.0)), u.x), u.y);
	}

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

	float4 Frag(Varyings input) : SV_Target
	{
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
					float4 result = float4(0,0,0,0);
					float2 uv = input.uv;
					float glitch = pow(abs(cos(T * Speed * 0.5) * 1.2 + 1.0), 1.2);
					glitch = saturate(glitch);
					float2 hp = float2(0.0, uv.y);
					float nh = noise(hp * 7.0 + T * Speed * 10.0) * (noise(hp + T * Speed * 0.3) * 0.8); //

					nh += noise(hp * 100.0 + T * Speed * 10.0) * Stretch; // 
					float rnd = 0.0;
					if (glitch > 0.0) { rnd = hash(uv); if (glitch < 1.0) { rnd *= glitch; } }
					nh *= glitch + rnd;
					half shiftR = 0.08 * mR;
					half shiftG = 0.07 * mG;
					half shiftB = mB;
					if (_FadeMultiplier > 0)
					{
						#if ALPHA_CHANNEL
							float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, input.uv).a);
						#else
							float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, input.uv).r);
						#endif
						Strength *= alpha_Mask;
					}
					float4 r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(nh, shiftR) * nh * Strength);
					float4 g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(nh - shiftG, 0.0) * nh * Strength);
					float4 b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(nh, shiftB) * nh * Strength);
					float4 kkk = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
					float4 col = float4(r.r, g.g, b.b, kkk.a + r.a + g.a + b.a);
					result = lerp(kkk,col,Fade);
					return result;
	}

		ENDHLSL

		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
			LOD 200
			Pass
		{
			HLSLPROGRAM

				#pragma vertex vert
				#pragma fragment Frag

			ENDHLSL
		}
	}
	FallBack "Diffuse"
}