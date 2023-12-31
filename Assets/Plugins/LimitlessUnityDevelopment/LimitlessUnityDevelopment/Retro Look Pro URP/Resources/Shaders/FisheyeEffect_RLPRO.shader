Shader "Hidden/Shader/FisheyeEffect_RLPRO"
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

	#pragma shader_feature VHS_FISHEYE_ON
	#pragma shader_feature VHS_FISHEYE_HYPERSPACE
	#define fixCoord (p - float2( 0.5 * ONE_X, 0.0)) 

	half cutoffX = 2.0;
	half cutoffY = 3.0;
	half cutoffFadeX = 100.0;
	half cutoffFadeY = 100.0;
	float fisheyeSize = 1.2;
	float fisheyeBend = 2.0;
	float time_ = 0.0;
	float ONE_X = 0.0;
	float ONE_Y = 0.0;
	float _Intensity;
	half fade;
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
	float2 fishEye(float2 uv, float size, float bend)
	{
		#if !VHS_FISHEYE_HYPERSPACE
				uv -= float2(0.5, 0.5);
				uv *= size * (1.0 / size + bend * uv.x * uv.x * uv.y * uv.y);
				uv += float2(0.5, 0.5);
		#endif 

		#if VHS_FISHEYE_HYPERSPACE
			float mx = bend / 50.0;
			float2 p = (uv * _ScreenParams.xy) / _ScreenParams.x;
			float prop = _ScreenParams.x / _ScreenParams.y;
			float2 m = float2(0.5, 0.5 / prop);
			float2 d = p - m;
			float r = sqrt(dot(d, d));
			float bind;
			float power = (2.0 * 3.141592 / (2.0 * sqrt(dot(m, m)))) * (mx - 0.5);
			if (power > 0.0) bind = sqrt(dot(m, m));
			else { if (prop < 1.0) bind = m.x; else bind = m.x; }
			if (power > 0.0)
				uv = m + normalize(d) * tan(r * power) * bind / tan(bind * power);
			else if (power < 0.0)
				uv = m + normalize(d) * atan(r * -power * 10.0) * bind / atan(-power * bind * 10.0);
			else uv = p;
			uv.y *= prop;
		#endif 
		return uv;
	}

	float4 CustomPostProcess(Varyings input) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
		half2 positionSS = input.uv ;
		float2 p = input.uv;
		ONE_X = 1.0 / _ScreenParams.x;
		ONE_Y = 1.0 / _ScreenParams.y;
		p = fishEye(p, fisheyeSize, fisheyeBend);
		float3 col = tex2D(_MainTex, p).rgb;
		half far;
		half2 hco = half2(ONE_X * cutoffX, ONE_Y * cutoffY);
		half2 sco = half2(ONE_X * cutoffFadeX, ONE_Y * cutoffFadeY);

		if (p.x <= (0.0 + hco.x) || p.x >= (1.0 - hco.x) ||
			p.y <= (0.0 + hco.y) || p.y >= (1.0 - hco.y))
		{
			col = half3(0.0, 0.0, 0.0);
		}
		else
		{
			if (
				(p.x > (0.0 + hco.x) && p.x < (0.0 + (sco.x + hco.x))) ||
				(p.x > (1.0 - (sco.x + hco.x)) && p.x < (1.0 - hco.x))
				) {
				if (p.x < 0.5)	far = (0.0 - hco.x + p.x) / (sco.x);
				else			far = (1.0 - hco.x - p.x) / (sco.x);
				col *= far;
			};
			if (
				(p.y > (0.0 + hco.y) && p.y < (0.0 + (sco.y + hco.y))) ||
				(p.y > (1.0 - (sco.y + hco.y)) && p.y < (1.0 - hco.y))
				) {
				if (p.y < 0.5)	far = (0.0 - hco.y + p.y) / (sco.y);
				else			far = (1.0 - hco.y - p.y) / (sco.y);
				col *= far;
			}
		}
		return float4(col.rgb,1);
	}

		ENDHLSL

		SubShader
	{
		Pass
		{
			Name "#VHSFisheye#"

			Cull Off ZWrite Off ZTest Always

			HLSLPROGRAM
				#pragma fragment CustomPostProcess
				#pragma vertex Vert
			ENDHLSL
		}
	}
	Fallback Off
}