Shader "Hidden/Shader/EdgeNoiseEffect_RLPRO"
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
	SAMPLER(_NoiseTexture);
	float _Intensity;
	float _OffsetNoiseX;
	half _OffsetNoiseY;
	half _NoiseBottomHeight;
	half _NoiseBottomIntensity;
	half tileX = 0;
	half tileY = 0;
    #pragma shader_feature top_ON
    #pragma shader_feature bottom_ON
    #pragma shader_feature left_ON
    #pragma shader_feature right_ON

    struct Attributes1
    {
        uint vertexID : SV_VertexID;
		float3 vertex : POSITION;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };
    struct Varyings1
    {
        float4 positionCS : SV_POSITION;
        float2 texcoord   : TEXCOORD0;
		float2 texcoordStereo   : TEXCOORD1;
        UNITY_VERTEX_OUTPUT_STEREO
    };
    Varyings1 Vert1(Attributes1 input)
    {
        Varyings1 output;
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
        output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
		output.texcoordStereo = output.texcoord + float2(_OffsetNoiseX - 0.2f, _OffsetNoiseY);
		output.texcoordStereo *= float2(tileY, tileX);
        return output;
    }
	

    float4 CustomPostProcess(Varyings1 input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
		half2 uv = input.texcoord;		
        float4 outColor = tex2D(_MainTex, uv);
        float condition = 0;
        float4 noise_bottom = float4(0,0,0,0);
        float val=1;
        int count = 1;
        _NoiseBottomHeight -= 0.099;
        #if top_ON
		        condition = saturate(floor(uv.y/ (1 - _NoiseBottomHeight)));
		        noise_bottom = tex2D(_NoiseTexture, input.texcoordStereo) * condition * _NoiseBottomIntensity;
                val = (1 - _NoiseBottomHeight) / uv.y;
		        outColor = lerp(outColor, noise_bottom, -noise_bottom * (val - 1.1));
        #endif
        #if bottom_ON
                condition = saturate(floor(_NoiseBottomHeight/ uv.y));
                noise_bottom = tex2D(_NoiseTexture, input.texcoordStereo) * condition * _NoiseBottomIntensity;
                val = uv.y/(_NoiseBottomHeight);
		        outColor += lerp(outColor, noise_bottom, -noise_bottom * (val - 1.0));
                count += 1;
                outColor /= 2;

        #endif
        #if left_ON
                condition = saturate(floor(_NoiseBottomHeight/ uv.x));
                noise_bottom = tex2D(_NoiseTexture, input.texcoordStereo) * condition * _NoiseBottomIntensity;
                val = uv.x/_NoiseBottomHeight;
		        outColor += lerp(outColor, noise_bottom, -noise_bottom * (val - 1.0));
                outColor /= 2;

        #endif
        #if right_ON
                condition = saturate(floor(uv.x / (1-_NoiseBottomHeight)))*3;
                noise_bottom = tex2D(_NoiseTexture, input.texcoordStereo) * condition * _NoiseBottomIntensity;
                val = (1-_NoiseBottomHeight) / uv.x;
		        outColor += lerp(outColor, noise_bottom, -noise_bottom * (val - 1.0));
                outColor /= 2;
        #endif

		return float4(pow(outColor.xyz, float3(1.0, 1.0, 1.0)), outColor.a);
    }

    ENDHLSL

    SubShader
    {
        Pass
        {
        Cull Off ZWrite Off ZTest Always

            HLSLPROGRAM
                #pragma fragment CustomPostProcess
                #pragma vertex Vert1
            ENDHLSL
        }
    }
    Fallback Off
}