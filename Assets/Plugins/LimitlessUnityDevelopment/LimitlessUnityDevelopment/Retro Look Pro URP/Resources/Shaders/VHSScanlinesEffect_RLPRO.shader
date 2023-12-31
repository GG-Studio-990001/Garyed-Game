Shader "Hidden/Shader/VHSScanlinesEffect_RLPRO"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
    HLSLINCLUDE

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		SAMPLER(_MainTex);
	TEXTURE2D(_Mask);
	SAMPLER(sampler_Mask);
	float _FadeMultiplier;
#pragma shader_feature ALPHA_CHANNEL

	float4 _ScanLinesColor;
	float _ScanLines;
	float speed;
	float fade;
	float _OffsetDistortion;
	float sferical;
	float barrel;
	float scale;
	float _OffsetColor;
	float2 _OffsetColorAngle;
	float Time;
		        struct Attributes
        {
            float4 positionOS       : POSITION;
            float2 uv               : TEXCOORD0;
        };

        struct Varyings
        {
            float2 uv        : TEXCOORD0;
            float4 positionCS : SV_POSITION;
            UNITY_VERTEX_OUTPUT_STEREO
        };
        Varyings Vert(Attributes input)
        {
            Varyings output = (Varyings)0;
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

            VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
            output.positionCS = vertexInput.positionCS;
            output.uv = input.uv;

            return output;
        }
		float2 FisheyeDistortion(float2 coord, float spherical, float barrel, float scale)
	{
		float2 h = coord.xy - float2(0.5, 0.5);
		float r2 = dot(h, h);
		float f = 1.0 + r2 * (spherical + barrel * sqrt(r2));
		return f * scale * h + 0.5;
	}

		float4 FragH(Varyings i) : SV_Target
		{
			float2 coord = FisheyeDistortion(i.uv, sferical, barrel, scale);
			half4 color = tex2D(_MainTex, i.uv);
			float lineSize = _ScreenParams.y * 0.005;
			float displacement = ((_Time.x / 4 * 1000) * speed) % _ScreenParams.y;
			float ps;
			ps = displacement + (coord.y * _ScreenParams.y / i.positionCS.w);
			float sc = i.uv.y;
			float4 result;
			result = ((uint)(ps / floor(_ScanLines * lineSize)) % 2 == 0) ? color : _ScanLinesColor;
			result += color * sc;
			if (_FadeMultiplier > 0)
			{
#if ALPHA_CHANNEL
				float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, i.uv).a);
#else
				float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, i.uv).r);
#endif
				fade *= alpha_Mask;
			}

			return lerp(color,result,fade);
		}

			float4 FragHD(Varyings i) : SV_Target
		{
			float2 coord = FisheyeDistortion(i.uv, sferical, barrel, scale);
			half4 color = tex2D(_MainTex, i.uv);
			float lineSize = _ScreenParams.y * 0.005;
			float displacement = ((_Time.x / 4 * 1000) * speed) % _ScreenParams.y;
			float ps;
			float yDist = frac(i.uv.y + cos((coord.x + _Time.x / 4) * 100) / _OffsetDistortion);
			ps = displacement + (yDist * _ScreenParams.y / i.positionCS.w);
			float sc = yDist;
			float4 result;
			result = ((uint)(ps / floor(_ScanLines * lineSize)) % 2 == 0) ? color : _ScanLinesColor;
			result += color * sc;
			if (_FadeMultiplier > 0)
			{
#if ALPHA_CHANNEL
				float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, i.uv).a);
#else
				float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, i.uv).r);
#endif
				fade *= alpha_Mask;
			}
			return lerp(color,result,fade);
		}

			float4 FragV(Varyings i) : SV_Target
		{
			float2 coord = FisheyeDistortion(i.uv, sferical, barrel, scale);
			half4 color = tex2D(_MainTex, i.uv);
			float lineSize = _ScreenParams.y * 0.005;
			float displacement = ((_Time.x / 4 * 1000) * speed) % _ScreenParams.y;
			float ps;
			ps = displacement + (coord.x * _ScreenParams.x / i.positionCS.w);
			float sc = i.uv.y;
			float4 result;
			result = ((uint)(ps / floor(_ScanLines * lineSize)) % 2 == 0) ? color : _ScanLinesColor;
			result += color * sc;
			if (_FadeMultiplier > 0)
			{
				float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, i.uv).a);
				fade *= alpha_Mask;
			}

			return lerp(color,result,fade);
		}

			float4 FragVD(Varyings i) : SV_Target
		{
			float2 coord = FisheyeDistortion(i.uv, sferical, barrel, scale);
			half4 color = tex2D(_MainTex, i.uv);
			float lineSize = _ScreenParams.y * 0.005;
			float displacement = ((_Time.x / 4 * 1000) * speed) % _ScreenParams.y;
			float ps;
			float yDist = frac(i.uv.x + cos((coord.x + _Time.x / 4) * 100) / _OffsetDistortion);
			ps = displacement + (yDist * _ScreenParams.x / i.positionCS.w);
			float sc = i.uv.y;
			float4 result;
			result = ((uint)(ps / floor(_ScanLines * lineSize)) % 2 == 0) ? color : _ScanLinesColor;
			result += color * sc;
			if (_FadeMultiplier > 0)
			{
				float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, i.uv).a);
				fade *= alpha_Mask;
			}

			return lerp(color,result,fade);
		}
    ENDHLSL

    SubShader
    {
		Cull Off ZWrite Off ZTest Always

			Pass
		{
			HLSLPROGRAM

				#pragma vertex Vert
				#pragma fragment FragH

			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM

				#pragma vertex Vert
				#pragma fragment FragHD

			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM

				#pragma vertex Vert
				#pragma fragment FragV

			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM

				#pragma vertex Vert
				#pragma fragment FragVD

			ENDHLSL
		}
    }
    Fallback Off
}