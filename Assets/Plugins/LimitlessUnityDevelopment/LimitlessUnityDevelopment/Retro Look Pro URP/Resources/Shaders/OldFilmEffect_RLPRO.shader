Shader "Hidden/Shader/OldFilmEffect_RLPRO"
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
    SAMPLER(_MainTex);
	TEXTURE2D(_Mask);
	SAMPLER(sampler_Mask);
	float _FadeMultiplier;
	#pragma shader_feature ALPHA_CHANNEL

	uniform float T;
	uniform float FPS;
	uniform float Contrast;
	uniform float Burn;
	uniform float SceneCut;
	uniform float Fade;

	float rand(float2 co)
	{
		return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
	}

	float rand(float c)
	{
		return rand(float2(c, 1.0));
	}

	float randomLine(float seed, float2 uv)
	{
		float aa = rand(seed + 1.0);
		float b = 0.01 * aa;
		float c = aa - 0.5;
		float l;
		if (aa > 0.2)
			l = pow(abs(aa * uv.x + b * uv.y + c), 0.125);
		else
			l = 2.0 - pow(abs(aa * uv.x + b * uv.y + c), 0.125);
		return lerp(0.5 - SceneCut, 1.0, l);
	}

	float randomBlotch(float seed, float2 uv)
	{
		float x = rand(seed);
		float y = rand(seed + 1.0);
		float s = 0.01 * rand(seed + 2.0);
		float2 p = float2(x, y) - uv;
		p.x *= 1;
		float aa = atan(p.y / p.x);
		float v = 1.0;
		float ss = s * s * (sin(6.2831 * aa * x) * 0.1 + 1.0);
		if (dot(p, p) < ss) v = 0.2;
		else v = pow(abs(dot(p, p) - ss), 1.0 / 16.0);
		return lerp(0.3 + 0.2 * (1.0 - (s / 0.02)) - SceneCut, 1.0, v);
	}


		float4 Frag0(Varyings i) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

		float2 uv = i.uv;
		float t = float(int(T * FPS));
		float2 suv = uv + 0.002 * float2(rand(t), rand(t + 23.0));
		float3 image = tex2D(_MainTex, float2(suv.x, suv.y) ).rgb;
		float luma = dot(float3(0.2126, 0.7152, 0.0722), image);
		float3 oldImage = luma * float3(0.7 + Burn, 0.7 + Burn / 2, 0.7) * Contrast;
		oldImage = oldImage * float3(0.7 + Burn, 0.7 + Burn / 8, 0.7) * Contrast;
		float randx = rand(t + 8.);
		float vI = 16.0 * (uv.x * (1.0 - uv.x) * uv.y * (1.0 - uv.y));
		vI *= lerp(0.7, 1.0, randx + .5);
		vI += 1.0 + 0.4 * randx;
		vI *= pow(abs(16.0 * uv.x * (1.0 - uv.x) * uv.y * (1.0 - uv.y)), 0.4);
		int l = int(8.0 * randx);
		if (0 < l) vI *= randomLine(t + 6.0 + 17. * float(0), uv);
		if (1 < l) vI *= randomLine(t + 6.0 + 17. * float(1), uv);
		int s = int(max(8.0 * rand(t + 18.0) - 2.0, 0.0));
		if (0 < s) vI *= randomBlotch(t + 6.0 + 19. * float(0), uv);
		if (1 < s) vI *= randomBlotch(t + 6.0 + 19. * float(1), uv);
		float4 result = float4(oldImage * vI, tex2D(_MainTex, float2(suv.x, suv.y)).a);


		if (_FadeMultiplier > 0)
		{
#if ALPHA_CHANNEL
			float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, i.uv).a);
#else
			float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, i.uv).r);
#endif
			Fade *= alpha_Mask;
		}

		result = lerp(result, tex2D(_MainTex, i.uv), 1 - Fade);
		return result;
	}



    ENDHLSL

    SubShader
    {
        Pass
        {
            Name "#NAME#"

			Cull Off ZWrite Off ZTest Always

            HLSLPROGRAM
                #pragma fragment Frag0
                #pragma vertex Vert
            ENDHLSL
        }
    }
    Fallback Off
}