Shader "Hidden/Shader/EdgeStretchEffect_RLPRO"
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
	float amplitude;
	float frequency;
	half _NoiseBottomHeight;
	float Time;
	half speed;
	#pragma shader_feature top_ON
	#pragma shader_feature bottom_ON
	#pragma shader_feature left_ON
	#pragma shader_feature right_ON
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
	float onOff(float a, float b, float c, float t)
	{
		return step(c, sin(t + a * cos(t * b)));
	}
	float2 wobble(float2 uv, float amplitude, float frequence, float speed)
	{
		float offset = amplitude * sin(uv.y * frequence * 20.0 + Time * speed);
		return float2((uv.x+(20 * _NoiseBottomHeight)) + offset, uv.y);
	}	
	float2 wobbleR(float2 uv, float amplitude, float frequence, float speed)
	{
		float offset = amplitude * onOff(2.1, 4.0, 0.3, Time * speed) * sin(uv.y * frequence * 20.0 + Time * speed);
		return float2((uv.x+(20 * _NoiseBottomHeight)) + offset, uv.y);
	}
	float2 wobbleV(float2 uv, float amplitude, float frequence, float speed)
	{
		float offset = amplitude * sin(uv.x * frequence * 20.0 + Time * speed);
		return float2((uv.y + (20 * _NoiseBottomHeight)) + offset, uv.x);
	}	
	float2 wobbleVR(float2 uv, float amplitude, float frequence, float speed)
	{
		float offset = amplitude * onOff(2.1, 4.0, 0.3, Time * speed) * sin(uv.x * frequence * 20.0 + Time * speed);
		return float2((uv.y + (20 * _NoiseBottomHeight)) + offset, uv.x);
	}
	float4 FragDist(Varyings i) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
		#if top_ON
			i.uv.y = min(i.uv.y, 1 - (wobble(i.uv, amplitude, frequency, speed).x * (_NoiseBottomHeight / 20)));

		#endif
		#if bottom_ON
			i.uv.y = max(i.uv.y, wobble(i.uv,amplitude, frequency, speed).x * (_NoiseBottomHeight/20));
		#endif
		#if left_ON
			i.uv.x = max(i.uv.x, wobbleV(i.uv, amplitude, frequency, speed).x * (_NoiseBottomHeight / 20));
		#endif
		#if right_ON
			i.uv.x = min(i.uv.x, 1 - (wobbleV(i.uv, amplitude, frequency, speed).x * (_NoiseBottomHeight / 20)));
		#endif
		half2 positionSS = i.uv ;
		half4 color = tex2D(_MainTex, positionSS);
		float exp = 1.0;
		return float4(pow(color.xyz, float3(exp, exp, exp)), color.a);
	}
	float4 FragDistRand(Varyings i) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);	
		#if top_ON
			i.uv.y = min(i.uv.y, 1 - (wobbleR(i.uv, amplitude, frequency, speed).x * (_NoiseBottomHeight / 20)));
		#endif
		#if bottom_ON
			i.uv.y = max(i.uv.y, wobbleR(i.uv, amplitude, frequency, speed).x * (_NoiseBottomHeight / 20));
		#endif
		#if left_ON
			i.uv.x = max(i.uv.x, wobbleVR(i.uv, amplitude, frequency, speed).x * (_NoiseBottomHeight / 20));
		#endif
		#if right_ON
			i.uv.x = min(i.uv.x, 1 - (wobbleVR(i.uv, amplitude, frequency, speed).x * (_NoiseBottomHeight / 20)));
		#endif
		half2 positionSS = i.uv  ;
		half4 color = tex2D(_MainTex, positionSS);
		float exp = 1.0;
		return float4(pow(color.xyz, float3(exp, exp, exp)), color.a);
	}
	float4 Frag(Varyings i) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
		 half2 positionSS = i.uv  ;
		#if top_ON
				 positionSS.y = min(positionSS.y, 1-(_NoiseBottomHeight / 2) - 0.01);
		#endif
		#if bottom_ON
				 positionSS.y = max(positionSS.y, (_NoiseBottomHeight / 2) - 0.01);
		#endif
		#if left_ON
				 positionSS.x = max(positionSS.x, (_NoiseBottomHeight / 2) - 0.01);
		#endif
		#if right_ON
				 positionSS.x = min(positionSS.x, 1-(_NoiseBottomHeight / 2) - 0.01);
		#endif

		half4 color = tex2D(_MainTex, positionSS);
		float exp = 1.0;
		return float4(pow(color.xyz, float3(exp, exp, exp)), color.a);
	}
    ENDHLSL

    SubShader
    {
		Cull Off ZWrite Off ZTest Always

			Pass
		{
			HLSLPROGRAM

				#pragma vertex Vert
				#pragma fragment FragDist

			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM

				#pragma vertex Vert
				#pragma fragment FragDistRand

			ENDHLSL
		}
			Pass
		{
			HLSLPROGRAM

				#pragma vertex Vert
				#pragma fragment Frag

			ENDHLSL
		}
    }
    Fallback Off
}