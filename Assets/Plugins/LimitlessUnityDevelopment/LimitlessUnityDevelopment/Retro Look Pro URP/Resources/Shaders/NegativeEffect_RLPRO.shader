Shader "Hidden/Shader/NegativeEffect_RLPRO"
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
#pragma shader_feature ALPHA_CHANNEL

	float _FadeMultiplier;
	uniform float T;
	uniform float Luminosity;
	uniform float Vignette;
	uniform float Negative;
	uniform float Contrast;
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
	float4 linearLight(float4 s, float4 d)
	{
		return 2.0 * s + d - 1.0 * Luminosity;
	}


		float4 Frag(Varyings i) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

	float2 uv = i.uv ;
	float4 col = tex2D(_MainTex, uv);
	col = lerp(col, 1 - col, Negative*1.5);
	float4 oldfilm = float4(1, 1, 1, 1);
	col *= pow(abs(0.1 * uv.x * (1.0 - uv.x) * uv.y * (1.0 - uv.y)), Contrast) * 1 + Vignette;
	col = dot(float4(0.2126, 0.7152, 0.0722, 1), col);
	col = linearLight(oldfilm, col);

	half4 colIn = tex2D(_MainTex, i.uv);
	float fd = 1;

	if (_FadeMultiplier > 0)
	{
#if ALPHA_CHANNEL
		float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, uv).a);
#else
		float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask,uv).r);
#endif
		fd *= alpha_Mask;
	}

	return lerp(colIn, float4(col.rgb,colIn.a), fd);
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