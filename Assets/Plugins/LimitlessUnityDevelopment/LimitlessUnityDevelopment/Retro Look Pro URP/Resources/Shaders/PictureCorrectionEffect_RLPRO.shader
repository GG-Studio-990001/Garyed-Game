Shader "Hidden/Shader/PictureCorrectionEffect_RLPRO"
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
#pragma shader_feature ALPHA_CHANNEL
	TEXTURE2D(_Mask);
	SAMPLER(sampler_Mask);
	float _FadeMultiplier;

	float signalAdjustY = 0.0;
	float signalAdjustI = 0.0;
	float signalAdjustQ = 0.0;

	float signalShiftY = 0.0;
	float signalShiftI = 0.0;
	float signalShiftQ = 0.0;
	float gammaCorection = 1.0;
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
	half3 rgb2yiq(half3 c) {
		return half3(
			(0.2989 * c.x + 0.5959 * c.y + 0.2115 * c.z),
			(0.5870 * c.x - 0.2744 * c.y - 0.5229 * c.z),
			(0.1140 * c.x - 0.3216 * c.y + 0.3114 * c.z)
			);
	};

	half3 yiq2rgb(half3 c) {
		return half3(
			(1.0 * c.x + 1.0 * c.y + 1.0 * c.z),
			(0.956 * c.x - 0.2720 * c.y - 1.1060 * c.z),
			(0.6210 * c.x - 0.6474 * c.y + 1.7046 * c.z)
			);
	};
		float4 Frag(Varyings i) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

		float3 signal = float3(0.0, 0.0, 0.0);
		float2 p = i.uv;
		signal = rgb2yiq(tex2D(_MainTex, p).xyz);
		signal.x += signalAdjustY;
		signal.y += signalAdjustI;
		signal.z += signalAdjustQ;
		signal.x *= signalShiftY;
		signal.y *= signalShiftI;
		signal.z *= signalShiftQ;

		half4 col1 = tex2D(_MainTex, p);
		half fade = 1;
		if (_FadeMultiplier > 0)
		{
#if ALPHA_CHANNEL
			float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, p).a);
#else
			float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, p).r);
#endif
			
			fade *= alpha_Mask;
		}

		float3 rgb = yiq2rgb(signal);
		if (gammaCorection != 1.0) rgb = pow(abs(rgb), gammaCorection);

		return lerp(col1,float4(rgb, col1.a),fade);



	}

    ENDHLSL

    SubShader
    {
			Pass
		{
			Name "#NAME#"

		Cull Off ZWrite Off ZTest Always

			HLSLPROGRAM
				#pragma fragment Frag
				#pragma vertex Vert
			ENDHLSL
		}
    }
    Fallback Off
}