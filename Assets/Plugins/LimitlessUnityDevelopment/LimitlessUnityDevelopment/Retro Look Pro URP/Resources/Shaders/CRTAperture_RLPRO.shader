Shader "Hidden/Shader/CRTAperture_RLPRO"
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
	
	#define FIX(c) max(abs(c), 1e-5)
	#define saturate(c) clamp(c, 0.0, 1.0)
	SAMPLER(_MainTex);
	TEXTURE2D(_Mask);
	SAMPLER(sampler_Mask);
	float _FadeMultiplier;
	#pragma shader_feature ALPHA_CHANNEL
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
	float GLOW_HALATION = 0.1;
	float GLOW_DIFFUSION = 0.05;
	float MASK_COLORS = 2.0;
	float MASK_STRENGTH = 0.3;
	float GAMMA_INPUT = 2.4;
	float GAMMA_OUTPUT = 2.4;
	float BRIGHTNESS = 1.5;

	float mod(float x, float y) {
		return	x - y * floor(x / y);
	}
	float fract(float x) {
		return  x - floor(x);
	}
	float2 fract(float2 x) {
		return  x - floor(x);
	}
	float4 fract(float4 x) {
		return  x - floor(x);
	}
	float3 TEX2D(float2 c) {
		return	pow(abs(tex2D(_MainTex, c).rgb), float3(GAMMA_INPUT, GAMMA_INPUT, GAMMA_INPUT)).xyz;
	}

	float3x3 get_color_matrix(float2 co, float2 dx)
	{
		return float3x3(TEX2D(co - dx), TEX2D(co), TEX2D(co + dx));
	}

	float3 blur(float3x3 m, float dist, float rad)
	{
		float3 x = float3(dist - 1.0, dist, dist + 1.0) / rad;
		float3 w = exp2(x * x * -1.0);

		return (m[0] * w.x + m[1] * w.y + m[2] * w.z) / (w.x + w.y + w.z);
	}

	float3 filter_gaussian(float2 co, float2 tex_size)
	{
		float2 dx = float2(1.0 / tex_size.x, 0.0);
		float2 dy = float2(0.0, 1.0 / tex_size.y);
		float2 pix_co = co * tex_size;
		float2 tex_co = (floor(pix_co) + 0.5) / tex_size;
		float2 dist = (fract(pix_co) - 0.5) * -1.0;
		float3x3 line0 = get_color_matrix(tex_co - dy, dx);
		float3x3 line1 = get_color_matrix(tex_co, dx);
		float3x3 line2 = get_color_matrix(tex_co + dy, dx);
		float3x3 column = float3x3(blur(line0, dist.x, 0.5), blur(line1, dist.x, 0.5), blur(line2, dist.x, 0.5));
		return blur(column, dist.y, 0.5);
	}

	float3 filter_lanczos(float2 co, float2 tex_size, float sharp)
	{
		tex_size.x *= sharp;

		float2 dx = float2(1.0 / tex_size.x, 0.0);
		float2 pix_co = co * tex_size - float2(0.5, 0.0);
		float2 tex_co = (floor(pix_co) + float2(0.5, 0.001)) / tex_size;
		float2 dist = fract(pix_co);
		float4 coef = PI * float4(dist.x + 1.0, dist.x, dist.x - 1.0, dist.x - 2.0);
		coef = FIX(coef);
		coef = 2.0 * sin(coef) * sin(coef / 2.0) / (coef * coef);
		coef /= dot(coef, float4(1.0, 1.0, 1.0, 1.0));
		float4 col1 = float4(TEX2D(tex_co), 1.0);
		float4 col2 = float4(TEX2D(tex_co + dx), 1.0);
		float4x4 fkfk = mul(coef.x, float4x4(col1, col1, col2, col2));
		float4x4 fkfks = mul(coef.y, float4x4(col1, col1, col2, col2));
		float4x4 fkfkb = mul(coef.z, float4x4(col1, col1, col2, col2));
		return float3(fkfk[0].x, fkfk[0].y, fkfk[0].z);
	}

	float3 mix(float3 x, float3 y, float3 a) {
		return float3(x * (1 - a) + y * a);
	}

	float3 get_mask_weight(float x)
	{
		float i = mod(floor(x), MASK_COLORS);
		if (i == 0.0) return mix(float3(1.0, 0.0, 1.0), float3(1.0, 0.0, 0.0), MASK_COLORS - 2.0);
		else if (i == 1.0) return float3(0.0, 1.0, 0.0);
		else return float3(0.0, 0.0, 1.0);
	}

	float4 Frag(Varyings i) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

		float2 pos = i.uv;
		half4 inputTex = tex2D(_MainTex, pos);
		float3 col_glow = filter_gaussian(pos, _ScreenParams.xy);
		float3 col_soft = filter_lanczos(pos, _ScreenParams.xy, 1);
		float3 col_sharp = filter_lanczos(pos, _ScreenParams.xy, 3);
		float3 col = sqrt(col_sharp * col_soft);
		col_glow = saturate(col_glow - col);
		col += col_glow * col_glow * GLOW_HALATION;
		col = mix(col, col * get_mask_weight(i.uv.x) * MASK_COLORS, MASK_STRENGTH);
		col += col_glow * GLOW_DIFFUSION;
		col = pow(abs(col * BRIGHTNESS), float3(1.0 / GAMMA_OUTPUT, 1.0 / GAMMA_OUTPUT, 1.0 / GAMMA_OUTPUT));
		float fd = 1;

		if (_FadeMultiplier > 0)
		{
			#if ALPHA_CHANNEL
						float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, pos).a);
			#else
						float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, pos).r);
			#endif
			fd *= alpha_Mask;
		}
		return lerp(inputTex,float4(col.rgb, inputTex.a),fd);
	}

		ENDHLSL

		SubShader
	{
		Pass
		{
			Name "#CRT_APERTURE#"

			Cull Off ZWrite Off ZTest Always

			HLSLPROGRAM
				#pragma fragment Frag
				#pragma vertex Vert
			ENDHLSL
		}
	}
	Fallback Off
}