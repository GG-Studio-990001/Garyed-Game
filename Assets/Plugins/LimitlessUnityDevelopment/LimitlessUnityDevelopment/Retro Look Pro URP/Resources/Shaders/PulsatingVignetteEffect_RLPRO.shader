Shader "Hidden/Shader/PulsatingVignetteEffect_RLPRO"
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

	float vignetteAmount = 1.0;
	float vignetteSpeed = 1.0;
	float Time = 0.0;
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
	float vignette(float2 uv, float t)
	{
		float vigAmt = 2.5 + 0.1 * sin(t + 5.0 * cos(t * 5.0));
		float c = (1.0 - vigAmt * (uv.y - 0.5) * (uv.y - 0.5)) * (1.0 - vigAmt * (uv.x - 0.5) * (uv.x - 0.5));
		c = pow(abs(c), vignetteAmount);
		return c;
	}

		float4 Frag(Varyings i) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

		float2 p = i.uv;
		float4 col = tex2D(_MainTex,p);
		col.rgb *= vignette(p, Time * vignetteSpeed);
		return half4(col);

	}

    ENDHLSL

    SubShader
    {
			Pass
		{
			Name "#PulsatingVignette#"

		Cull Off ZWrite Off ZTest Always

			HLSLPROGRAM
				#pragma fragment Frag
				#pragma vertex Vert
			ENDHLSL
		}
    }
    Fallback Off
}