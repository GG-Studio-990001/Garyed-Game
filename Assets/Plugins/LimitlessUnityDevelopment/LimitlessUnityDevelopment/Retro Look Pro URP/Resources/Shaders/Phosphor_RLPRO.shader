Shader "Hidden/Shader/Phosphor_RLPRO"
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
	SAMPLER(_Tex);
	TEXTURE2D(_Mask);
	SAMPLER(sampler_Mask);
	float2 mouse;
	float _FadeMultiplier;

	float speed = 10.00;
	half fade;
	float T;
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
	float fract(float x) {
		return  x - floor(x);
	}
	float2 fract(float2 x) {
		return  x - floor(x);
	}

	float random(float2 noise)
	{
		return fract(sin(dot(noise.xy, float2(0.0001, 98.233))) * 925895933.14159265359);
	}

	float random_color(float noise)
	{
		return frac(sin(noise));
	}
	float4 Frag0(Varyings i) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
		half2 uv = i.uv;
		float4 col =  tex2D(_MainTex,uv);
		float4 result = col+  tex2D(_Tex, uv);
		if (_FadeMultiplier > 0)
		{
			float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, i.uv).a);
			fade *= alpha_Mask;
		}

		return lerp(col,result,fade);
	
	}

	half4 Frag(Varyings i) : SV_Target
	{
		half2 uv = fract(i.uv.xy / 12 * ((T * speed)));
		half4 color = float4(random(uv.xy), random(uv.xy), random(uv.xy), random(uv.xy));

		color.r *= random_color(sin(T * speed));
		color.g *= random_color(cos(T * speed));
		color.b *= random_color(tan(T * speed));

		return color;

	}

		ENDHLSL

		SubShader
	{
		Pass
		{
			Name "#MixPass#"

		Cull Off ZWrite Off ZTest Always

			HLSLPROGRAM
				#pragma fragment Frag0
				#pragma vertex Vert
				#pragma target 3.0
			ENDHLSL
		}
			Pass
		{
			Name "#NoisePass#"

		Cull Off ZWrite Off ZTest Always

			HLSLPROGRAM
				#pragma fragment Frag
				#pragma vertex Vert
				#pragma target 3.0
			ENDHLSL
		}
	}
	Fallback Off
}