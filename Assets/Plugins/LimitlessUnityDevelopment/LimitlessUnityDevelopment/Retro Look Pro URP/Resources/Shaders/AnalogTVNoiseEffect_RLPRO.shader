Shader "Hidden/Shader/AnalogTVNoiseEffect_RLPRO"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }

    HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
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

    TEXTURE2D(_MainTex);
    SAMPLER(sampler_MainTex);
    TEXTURE2D(_Mask);
    SAMPLER(sampler_Mask);
    TEXTURE2D(_Pattern);
    SAMPLER(sampler_Pattern);
    #pragma shader_feature ALPHA_CHANNEL
    float _FadeMultiplier;

	float _Intensity;
	float TimeX;
	half _Fade;
	half barHeight = 6.;
	half barOffset = 0.6;
	half barSpeed = 2.6;
	half barOverflow = 1.2;
	half edgeCutOff;
	half cut;
	half _OffsetNoiseX;
	half _OffsetNoiseY;
	half4 _MainTex_ST;
	half tileX = 0;
	half tileY = 0;
	half angle;
	uint horizontal;

    Varyings1 Vert1(Attributes1 input)
    {
        Varyings1 output;
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
        output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
		float2 pivot = float2(0.5, 0.5);
		// Rotation Matrix
		float cosAngle = cos(angle);
		float sinAngle = sin(angle);
		float2x2 rot = float2x2(cosAngle, -sinAngle, sinAngle, cosAngle);
		// Rotation consedering pivot
		float2 uv = output.positionCS.xy;
		float2 sfsf = mul(rot, uv);
		output.texcoordStereo = sfsf + output.texcoord + float2(_OffsetNoiseX - 0.2f, _OffsetNoiseY), _ScreenSize.zw * float2(tileY, tileX);
		output.texcoordStereo *= float2(tileY, tileX);
        return output;
    }

    float4 CustomPostProcess(Varyings1 input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        half2 positionSS = input.texcoord ;
        float4 outColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, positionSS);
		float4 pat = SAMPLE_TEXTURE2D(_Pattern, sampler_Pattern, input.texcoordStereo.xy);
		float4 col = outColor;
		float direction = horizontal > 0 ? input.texcoord.y : input.texcoord.x;
		float bar = floor(edgeCutOff + sin(direction * barHeight + TimeX * barSpeed) * 50);
		float f = clamp(bar * 0.03, 0, 1);
        if (_FadeMultiplier > 0)
        {
            #if ALPHA_CHANNEL
                        float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, positionSS).a);
            #else
                        float alpha_Mask = step(0.0001, SAMPLE_TEXTURE2D(_Mask, sampler_Mask, positionSS).r);
            #endif
            _Fade *= alpha_Mask;
        }

		col = lerp(pat, col, f);
		col = lerp(outColor, col, smoothstep(col.r - cut, 0, 1) * _Fade);
		return float4(col.rgb, outColor.a);
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